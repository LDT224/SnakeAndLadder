using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainGameController : MonoBehaviour
{
    public int numRoll;
    public int currentPlayer = 0;
    private List<GameObject> players = new List<GameObject>();
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    Color[] playerColor;
    private int numPlayer;
    private int[] currentPos = new int[] { 0, 0, 0, 0 };
    private List<GameObject> maps = new List<GameObject>();
    public IDictionary<int,int> snakes = new Dictionary<int,int>();
    public IDictionary<int,int> ladders = new Dictionary<int,int>();

    private MapController mapController;
    private MainUIController mainUIController;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ChangeStatus(GameManager.GameStatus.Play);      

        mainUIController = FindObjectOfType<MainUIController>();
        mapController = FindObjectOfType<MapController>();
        maps = mapController.boxs;

        numPlayer = GameManager.Instance.numPlayer;
        SpawnPlayer();
        SetCurrentPlayer();
    }

    public void SpawnPlayer()
    {
        for (int i = 0; i < numPlayer; i++)
        {
            GameObject player = Instantiate(playerPrefab,Vector3.zero, Quaternion.identity);
            players.Add(player);
            maps[0].GetComponent<BoxController>().CheckSlotPlayer(players[i]);
            player.GetComponent<SpriteRenderer>().color = playerColor[i];
        }
    }
    public void SetCurrentPlayer()
    {
        mainUIController.ChangeTurnTxt(currentPlayer +1);
    }

    public void NextTurn()
    {
        currentPlayer = (currentPlayer + 1) % players.Count;
        SetCurrentPlayer();
    }

    public void PlayerMove(int currentPlayer,int numRoll)
    {
        int newPos = currentPos[currentPlayer] + numRoll;
        if(newPos < GameManager.Instance.totalMap -1)
        {
            currentPos[currentPlayer] = newPos;
            maps[newPos].GetComponent<BoxController>().CheckSlotPlayer(players[currentPlayer]);
            StartCoroutine(CheckBoxMoveIn(currentPlayer, newPos));
            Debug.Log("Player " + currentPlayer + "move to " + currentPos[currentPlayer]);
        }
        else if(newPos == GameManager.Instance.totalMap - 1)
        {
            currentPos[currentPlayer] = newPos;
            maps[newPos].GetComponent<BoxController>().CheckSlotPlayer(players[currentPlayer]);
            Debug.Log("Player: " + currentPlayer + "WINNNNN!!!!");
        }
        else
        {
            Debug.Log("Over map =>> reRoll");
        }
    }

    private IEnumerator CheckBoxMoveIn(int player ,int pos)
    {
        yield return new WaitForSeconds(1.0f); // Wait 1 sec
        int playerInTxt = player + 1;

        if (maps[pos].GetComponent<BoxController>().status == BoxController.BoxStatus.SnakeHead)
        {
            if (snakes.ContainsKey(pos))
            {
                currentPos[player] = snakes[pos];
                maps[snakes[pos]].GetComponent<BoxController>().CheckSlotPlayer(players[player]);
            }
            mainUIController.statusTxt.text = "Player " + playerInTxt + " in snake box";
        }

        if (maps[pos].GetComponent<BoxController>().status == BoxController.BoxStatus.LadderBottom)
        {
            if (ladders.ContainsKey(pos))
            {
                currentPos[player] = ladders[pos];
                maps[ladders[pos]].GetComponent<BoxController>().CheckSlotPlayer(players[player]);
            }
            mainUIController.statusTxt.text = "Player " + playerInTxt + " in ladder box";
        }

        if (maps[pos].GetComponent<BoxController>().status == BoxController.BoxStatus.Question)
        {
            mainUIController.statusTxt.text = "Player " + playerInTxt + " in question box";
            mainUIController.OnQuestion();
        }

        if (maps[pos].GetComponent<BoxController>().status == BoxController.BoxStatus.BattleQuestion)
        {
            mainUIController.statusTxt.text = "Player " + playerInTxt + " in battle question box";
            mainUIController.OnQuestion();
        }

        if (maps[pos].GetComponent<BoxController>().status == BoxController.BoxStatus.MiniGame)
        {
            mainUIController.statusTxt.text = "Player " + playerInTxt + " in mini game box";
        }

        if (maps[pos].GetComponent<BoxController>().status == BoxController.BoxStatus.BattleMiniGame)
        {
            mainUIController.statusTxt.text = "Player " + playerInTxt + " in battle mini game box";
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.canRoll == false) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousepos2d = new Vector2(mousepos.x, mousepos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousepos2d, Vector2.zero);

            if (!hit.collider) return;

            if (hit.collider.CompareTag("Dice"))
            {
                numRoll = 1 + Random.Range(0, 6);
                hit.collider.gameObject.GetComponent<DiceController>().Roll(numRoll);
                GameManager.Instance.ChangeStatus(GameManager.GameStatus.InTurn);
                Debug.Log( "roll: " + numRoll);
            }
        }
    }
}
