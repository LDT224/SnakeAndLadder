using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;

public class MainUIController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField]
    private Text turnTxt;
    public Text statusTxt;

    [SerializeField]
    private Text timeTxt;
    private int time;
    [SerializeField]
    private Image timerFillImg;

    [SerializeField]
    private GameObject questionUI;
    public Text questionTxt;
    public Button aBtn;
    public Button bBtn;
    public Button cBtn;
    public Button dBtn;

    private Coroutine myCoroutine;
    private List<string> playerInTurn;
    private string localPlayerID;
    private int currentPlayer;

    private MainGameController gameController;

    void Start()
    {
        statusTxt.text = "Game start";
        gameController = FindObjectOfType<MainGameController>();
    }
    
    public void ChangeTurnTxt(int _currentPlayer)
    {
        currentPlayer = _currentPlayer;
        turnTxt.text = PhotonNetwork.PlayerList[_currentPlayer].NickName + " turn";
    }

    public void OnQuestion(List<string> _playerInTurn)
    {
        questionUI.SetActive(true);

        localPlayerID = PhotonNetwork.LocalPlayer.UserId;
        playerInTurn = new List<string>(_playerInTurn);

        if (playerInTurn.Contains(localPlayerID))
        {
            aBtn.interactable = true;
            bBtn.interactable = true;
            cBtn.interactable = true;
            dBtn.interactable = true;
        }
        else
        {
            aBtn.interactable = false;
            bBtn.interactable = false;
            cBtn.interactable = false;
            dBtn.interactable = false;
        }
        time = GameManager.Instance.timeAnswerQuestion;
        myCoroutine = StartCoroutine(UpdateTimer());

    }

    private IEnumerator UpdateTimer()
    {
        while(time >= 0)
        {
            timeTxt.text = time.ToString();
            timerFillImg.fillAmount = Mathf.InverseLerp(0, GameManager.Instance.timeAnswerQuestion, time);
            time--;
            yield return new WaitForSeconds(1f);
        }

        EndTime();
    }
    public void EndTime()
    {
        questionUI.SetActive(false);
        questionTxt.text = "";
        aBtn.GetComponentInChildren<Text>().text = "";
        bBtn.GetComponentInChildren<Text>().text = "";
        cBtn.GetComponentInChildren<Text>().text = "";
        dBtn.GetComponentInChildren<Text>().text = "";
        gameController.WrongAnswer();

        if (localPlayerID == PhotonNetwork.PlayerList[currentPlayer].UserId)
            GameManager.Instance.ChangeStatus(GameManager.GameStatus.EndTurn);
    }

    [PunRPC]
    void RPC_Answered()
    {
        questionUI.SetActive(false);
        questionTxt.text = "";
        aBtn.GetComponentInChildren<Text>().text = "";
        bBtn.GetComponentInChildren<Text>().text = "";
        cBtn.GetComponentInChildren<Text>().text = "";
        dBtn.GetComponentInChildren<Text>().text = "";

        StopCoroutine(myCoroutine);
    }
    public void Answered()
    {
        photonView.RPC("RPC_Answered", RpcTarget.All);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
