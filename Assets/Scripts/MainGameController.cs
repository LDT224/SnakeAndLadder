using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;

public class MainGameController : MonoBehaviourPunCallbacks
{
    public int numRoll;
    public int currentPlayer = 0;
    private List<GameObject> players = new List<GameObject>();
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    Color[] playerColor;
    private int[] currentPos = new int[] { 0, 0, 0, 0 };
    private List<GameObject> maps = new List<GameObject>();
    public IDictionary<int,int> snakes = new Dictionary<int,int>();
    public IDictionary<int,int> ladders = new Dictionary<int,int>();
    [SerializeField]
    private TextAsset listQuestion;
    private string answer;
    private List<int> questionShowed = new List<int>();
    private string[] data;

    private MapController mapController;
    private MainUIController mainUIController;

    private string localPlayerID;

    private int numberOfDiceRolls;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ChangeStatus(GameManager.GameStatus.Play);      

        mainUIController = FindObjectOfType<MainUIController>();
        mapController = FindObjectOfType<MapController>();
        maps = mapController.boxs;

        localPlayerID = PhotonNetwork.LocalPlayer.UserId;

        SpawnPlayer();
        SetCurrentPlayer();

        numberOfDiceRolls = 0;
    }

    public void SpawnPlayer()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject player = Instantiate(playerPrefab,Vector3.zero, Quaternion.identity);
            players.Add(player);
            maps[0].GetComponent<BoxController>().CheckSlotPlayer(players[i]);
            player.GetComponent<SpriteRenderer>().color = playerColor[i];
        }
    }
    public void SetCurrentPlayer()
    {
        mainUIController.ChangeTurnTxt(currentPlayer);
        GameManager.Instance.ChangeStatus(GameManager.GameStatus.Play);
    }

    public void NextTurn()
    {
        currentPlayer = (currentPlayer + 1) % players.Count;
        SetCurrentPlayer();
    }

    [PunRPC]
    void RPC_PlayerMove(int currentPlayer,int numRoll)
    {
        int newPos = currentPos[currentPlayer] + numRoll;
        if(newPos < GameManager.Instance.totalMap -1)
        {
            maps[newPos].GetComponent<BoxController>().CheckSlotPlayer(players[currentPlayer]);
            StartCoroutine(CheckBoxMoveIn(currentPlayer, newPos));
        }
        else if(newPos == GameManager.Instance.totalMap - 1)
        {
            currentPos[currentPlayer] = newPos;
            maps[newPos].GetComponent<BoxController>().CheckSlotPlayer(players[currentPlayer]);
            if(localPlayerID == PhotonNetwork.PlayerList[currentPlayer].UserId)
                GameManager.Instance.ChangeStatus(GameManager.GameStatus.Finish);
            PlayfabController.instance.GetPlayerInLeadboard();
            StartCoroutine(EndMatch(currentPlayer));
            Debug.Log("Player: " + currentPlayer + "WINNNNN!!!!");
        }
        else
        {
            Debug.Log("Over map =>> reRoll");
            if (localPlayerID == PhotonNetwork.PlayerList[currentPlayer].UserId)
                GameManager.Instance.ChangeStatus(GameManager.GameStatus.EndTurn);
        }
    }

    private IEnumerator PlayerMove(int currentPlayer, int numRoll)
    {
        yield return new WaitForSeconds(2f);
        photonView.RPC("RPC_PlayerMove", RpcTarget.All, currentPlayer, numRoll);
    }
    private IEnumerator CheckBoxMoveIn(int currentPlayer ,int pos)
    {
        yield return new WaitForSeconds(1.5f); 
        List<string> playerInTurn = new List<string>();
        playerInTurn.Add(PhotonNetwork.PlayerList[currentPlayer].UserId);

        if (maps[pos].GetComponent<BoxController>().status == BoxController.BoxStatus.SnakeHead)
        {
            AudioManager.instance.PlaySFX("Bite");
            if (snakes.ContainsKey(pos))
            {
                currentPos[currentPlayer] = snakes[pos];
                maps[snakes[pos]].GetComponent<BoxController>().CheckSlotPlayer(players[currentPlayer]);
            }
            mainUIController.statusTxt.text = PhotonNetwork.PlayerList[currentPlayer].NickName + " is bitten by a snake!";
            if (localPlayerID == PhotonNetwork.PlayerList[currentPlayer].UserId)
                GameManager.Instance.ChangeStatus(GameManager.GameStatus.EndTurn);
        }
        else if (maps[pos].GetComponent<BoxController>().status == BoxController.BoxStatus.LadderBottom)
        {
            AudioManager.instance.PlaySFX("Ladder");
            if (ladders.ContainsKey(pos))
            {
                currentPos[currentPlayer] = ladders[pos];
                maps[ladders[pos]].GetComponent<BoxController>().CheckSlotPlayer(players[currentPlayer]);
            }
            mainUIController.statusTxt.text = PhotonNetwork.PlayerList[currentPlayer].NickName + " is climbing up a ladder!";
            if (localPlayerID == PhotonNetwork.PlayerList[currentPlayer].UserId)
                GameManager.Instance.ChangeStatus(GameManager.GameStatus.EndTurn);
        }
        else if (maps[pos].GetComponent<BoxController>().status == BoxController.BoxStatus.Question)
        {
            AudioManager.instance.PlaySFX("Question");
            mainUIController.statusTxt.text = PhotonNetwork.PlayerList[currentPlayer].NickName + " in question box";
            mainUIController.playerTxt.text = PhotonNetwork.PlayerList[currentPlayer].NickName;
            mainUIController.ranPlayerTxt.text = "";
            if (localPlayerID == PhotonNetwork.PlayerList[currentPlayer].UserId)
                GetQuestionData();
            mainUIController.OnQuestion(playerInTurn);
        }
        else if (maps[pos].GetComponent<BoxController>().status == BoxController.BoxStatus.BattleQuestion)
        {
            AudioManager.instance.PlaySFX("Question");
            mainUIController.statusTxt.text = PhotonNetwork.PlayerList[currentPlayer].NickName + " in battle question box";
            if (localPlayerID == PhotonNetwork.PlayerList[currentPlayer].UserId)
                GetQuestionData();
            while (true)
            {
                int ranPlayer = Random.Range(0, PhotonNetwork.PlayerList.Count());

                if (ranPlayer != currentPlayer)
                {
                    playerInTurn.Add(PhotonNetwork.PlayerList[ranPlayer].UserId);
                    mainUIController.playerTxt.text = PhotonNetwork.PlayerList[currentPlayer].NickName + " VS";
                    mainUIController.ranPlayerTxt.text = PhotonNetwork.PlayerList[ranPlayer].NickName;
                    break;
                }
            }
            mainUIController.OnQuestion(playerInTurn);
        }
        else if (maps[pos].GetComponent<BoxController>().status == BoxController.BoxStatus.MiniGame)
        {
            mainUIController.statusTxt.text = PhotonNetwork.PlayerList[currentPlayer].NickName + " in mini game box";
            currentPos[currentPlayer] = pos;
            if (localPlayerID == PhotonNetwork.PlayerList[currentPlayer].UserId)
                GameManager.Instance.ChangeStatus(GameManager.GameStatus.EndTurn);
        }
        else if (maps[pos].GetComponent<BoxController>().status == BoxController.BoxStatus.BattleMiniGame)
        {
            mainUIController.statusTxt.text = PhotonNetwork.PlayerList[currentPlayer].NickName + " in battle mini game box";
            currentPos[currentPlayer] = pos;
            if (localPlayerID == PhotonNetwork.PlayerList[currentPlayer].UserId)
                GameManager.Instance.ChangeStatus(GameManager.GameStatus.EndTurn);
        }
        else
        {
            mainUIController.statusTxt.text = "Good luck!";
            currentPos[currentPlayer] = pos;
            if (localPlayerID == PhotonNetwork.PlayerList[currentPlayer].UserId)
                GameManager.Instance.ChangeStatus(GameManager.GameStatus.EndTurn);
        }
    }

    public void GetQuestionData()
    {
        string a = "";
        string b = "";
        string c = "";
        string d = "";
        string question = "";
        data = listQuestion.text.Split(new string[] { ";", "\n" }, System.StringSplitOptions.None);   

        while (mainUIController.questionTxt.text == "")
        {
            int r = Random.Range(1, data.Length);

            if (questionShowed.Contains(r) == false)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == r.ToString())
                    {
                        mainUIController.questionTxt.text = "Question: " + data[i + 1];
                        question = "Question: " + data[i + 1];
                        answer = data[i + 2];
                        List<int> answerPicked = new List<int>();
                        while (mainUIController.aBtn.GetComponentInChildren<Text>().text == "")
                        {
                            int ran = Random.Range(2, 6);
                            if (answerPicked.Contains(ran) == false)
                            {
                                mainUIController.aBtn.GetComponentInChildren<Text>().text = "A: " + data[i + ran];
                                a = "A: " + data[i + ran];
                                answerPicked.Add(ran);
                            }
                            else
                                continue;
                        }

                        while (mainUIController.bBtn.GetComponentInChildren<Text>().text == "")
                        {
                            int ran = Random.Range(2, 6);
                            if (answerPicked.Contains(ran) == false)
                            {
                                mainUIController.bBtn.GetComponentInChildren<Text>().text = "B: " + data[i + ran];
                                b = "B: " + data[i + ran];
                                answerPicked.Add(ran);
                            }
                            else
                                continue;
                        }

                        while (mainUIController.cBtn.GetComponentInChildren<Text>().text == "")
                        {
                            int ran = Random.Range(2, 6);
                            if (answerPicked.Contains(ran) == false)
                            {
                                mainUIController.cBtn.GetComponentInChildren<Text>().text = "C: " + data[i + ran];
                                c = "C: " + data[i + ran];
                                answerPicked.Add(ran);
                            }
                            else
                                continue;
                        }

                        while (mainUIController.dBtn.GetComponentInChildren<Text>().text == "")
                        {
                            int ran = Random.Range(2, 6);
                            if (answerPicked.Contains(ran) == false)
                            {
                                mainUIController.dBtn.GetComponentInChildren<Text>().text = "D: " + data[i + ran];
                                d = "D: " + data[i + ran];
                                answerPicked.Clear();
                            }
                            else
                                continue;
                        }
                    }
                }
                photonView.RPC("RPC_AddQuestionShowedList", RpcTarget.All, r);
            }
            else
                continue;
        }

        photonView.RPC("RPC_SendQuestionData", RpcTarget.Others, a, b, c, d, question, answer);
    }

    [PunRPC]
    void RPC_SendQuestionData(string a, string b, string c, string d, string question, string _answer)
    {
        mainUIController.aBtn.GetComponentInChildren<Text>().text = a;
        mainUIController.bBtn.GetComponentInChildren<Text>().text = b;
        mainUIController.cBtn.GetComponentInChildren<Text>().text = c;
        mainUIController.dBtn.GetComponentInChildren<Text>().text = d;
        mainUIController.questionTxt.text = question;
        answer = _answer;
    }

    [PunRPC]
    void RPC_AddQuestionShowedList(int ques)
    {
        questionShowed.Add(ques);
    }

    public void CheckAnswer(Button button)
    {
        if(button.GetComponentInChildren<Text>().text.Substring(3) == answer)
        {
            if (localPlayerID == PhotonNetwork.PlayerList[currentPlayer].UserId)
            {
                currentPos[currentPlayer] = currentPos[currentPlayer] + numRoll;
                RightAnswer(currentPos[currentPlayer]);
            }
            else
                WrongAnswer();
        }
        else
        {
            if (localPlayerID == PhotonNetwork.PlayerList[currentPlayer].UserId)
            {
                WrongAnswer();
            }
            else
            {
                currentPos[currentPlayer] = currentPos[currentPlayer] + numRoll;
                RightAnswer(currentPos[currentPlayer]);
            }
        }
        answer = "";
        mainUIController.Answered();
        GameManager.Instance.ChangeStatus(GameManager.GameStatus.EndTurn);
    }

    public void RightAnswer(int newPos)
    {
        photonView.RPC("RPC_RightAnswer", RpcTarget.Others, newPos);
    }
    [PunRPC]
    void RPC_RightAnswer(int newPos)
    {
        AudioManager.instance.PlaySFX("Correct");
        mainUIController.statusTxt.text = PhotonNetwork.PlayerList[currentPlayer].NickName + " has won, the answer is: " + answer;
        currentPos[currentPlayer] = newPos;
    }

    public void WrongAnswer()
    {
        photonView.RPC("RPC_WrongAnswer",RpcTarget.All);
    }
    [PunRPC]
    void RPC_WrongAnswer()
    {
        AudioManager.instance.PlaySFX("Wrong");
        mainUIController.statusTxt.text = PhotonNetwork.PlayerList[currentPlayer].NickName + " has lost, the answer is: " + answer;
        maps[currentPos[currentPlayer]].GetComponent<BoxController>().CheckSlotPlayer(players[currentPlayer]);
    }

    IEnumerator EndMatch(int winner)
    {
        yield return new WaitForSeconds(2f);
        if (localPlayerID == PhotonNetwork.PlayerList[winner].UserId)
        {
            AudioManager.instance.PlaySFX("Win");
            mainUIController.WinMatch();
            int score = 100 - numberOfDiceRolls;
            mainUIController.scoreTxt.text = "+ " + score;
            PlayfabController.instance.SendLeaderboard(score);
        }
        else
        {
            AudioManager.instance.PlaySFX("Lose");
            mainUIController.LoseMatch();
            int score = GameManager.Instance.totalMap - currentPos[PhotonNetwork.LocalPlayer.ActorNumber - 1] - 1;
            mainUIController.scoreTxt.text = "- " + score;
            PlayfabController.instance.SendLeaderboard(score * -1);
        }

        mainUIController.totalScoreTxt.text = PlayerPrefs.GetString("TotalScore");
        mainUIController.rankTxt.text = PlayerPrefs.GetString("Rank");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.canRoll == false) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (localPlayerID != PhotonNetwork.PlayerList[currentPlayer].UserId)
            {
                mainUIController.statusTxt.text = "It's not your turn!";
                return;
            }
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousepos2d = new Vector2(mousepos.x, mousepos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousepos2d, Vector2.zero);

            if (!hit.collider) return;

            if (hit.collider.CompareTag("Dice"))
            {
                numRoll = 1 + Random.Range(0, 6);
                hit.collider.gameObject.GetComponent<DiceController>().Roll(numRoll);
                StartCoroutine(PlayerMove(currentPlayer, numRoll));
                if (localPlayerID == PhotonNetwork.PlayerList[currentPlayer].UserId && GameManager.Instance.status != GameManager.GameStatus.Finish)
                    GameManager.Instance.ChangeStatus(GameManager.GameStatus.InTurn);
                numberOfDiceRolls++;
            }
        }
    }
}
