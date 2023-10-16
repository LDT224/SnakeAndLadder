using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
    public Button aTxt;
    public Button bTxt;
    public Button cTxt;
    public Button dTxt;

    private Coroutine myCoroutine;

    void Start()
    {
        statusTxt.text = "Game start";
    }
    
    public void ChangeTurnTxt(int currentPlayer)
    {
        turnTxt.text = PhotonNetwork.PlayerList[currentPlayer].NickName + " turn";
    }

    public void OnQuestion()
    {
        questionUI.SetActive(true);
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
        aTxt.GetComponentInChildren<Text>().text = "";
        bTxt.GetComponentInChildren<Text>().text = "";
        cTxt.GetComponentInChildren<Text>().text = "";
        dTxt.GetComponentInChildren<Text>().text = "";

        GameManager.Instance.ChangeStatus(GameManager.GameStatus.EndTurn);
    }
    public void Anserwed()
    {
        questionUI.SetActive(false);
        questionTxt.text = "";
        aTxt.GetComponentInChildren<Text>().text = "";
        bTxt.GetComponentInChildren<Text>().text = "";
        cTxt.GetComponentInChildren<Text>().text = "";
        dTxt.GetComponentInChildren<Text>().text = "";

        StopCoroutine(myCoroutine);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
