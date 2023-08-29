using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Text aTxt;
    public Text bTxt;
    public Text cTxt;
    public Text dTxt;
    
    void Start()
    {
        statusTxt.text = "Game start";
    }
    
    public void ChangeTurnTxt(int currentPlayer)
    {
        turnTxt.text = "Player " + currentPlayer + " turn";
    }

    public void OnQuestion()
    {
        questionUI.SetActive(true);
        time = GameManager.Instance.timeAnswerQuestion;
        StartCoroutine(UpdateTimer());
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
        aTxt.text = "";
        bTxt.text = "";
        cTxt.text = "";
        dTxt.text = "";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
