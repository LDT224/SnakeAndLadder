using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;

public class MainUIController : MonoBehaviour
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

    void Start()
    {
        statusTxt.text = "Game start";
    }
    
    public void ChangeTurnTxt(int currentPlayer)
    {
        turnTxt.text = PhotonNetwork.PlayerList[currentPlayer].NickName + " turn";
    }

    public void OnQuestion(List<string> playerInTurn)
    {
        questionUI.SetActive(true);

        string currentPlayerId = PhotonNetwork.LocalPlayer.UserId;

        if (playerInTurn.Contains(currentPlayerId))
        {
            aBtn.interactable = true;
            bBtn.interactable = true;
            cBtn.interactable = true;
            dBtn.interactable = true;
            Debug.Log(playerInTurn);
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

        GameManager.Instance.ChangeStatus(GameManager.GameStatus.EndTurn);
    }

    public void Answered()
    {
        questionUI.SetActive(false);
        questionTxt.text = "";
        aBtn.GetComponentInChildren<Text>().text = "";
        bBtn.GetComponentInChildren<Text>().text = "";
        cBtn.GetComponentInChildren<Text>().text = "";
        dBtn.GetComponentInChildren<Text>().text = "";

        StopCoroutine(myCoroutine);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
