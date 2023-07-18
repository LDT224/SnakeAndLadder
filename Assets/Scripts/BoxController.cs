using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class BoxController : MonoBehaviour
{
    public enum BoxStatus
    {
        Init,
        Question,
        BattleQuestion,
        MiniGame,
        BattleMiniGame,
        SnakeHead,
        SnakeTail,
        LadderTop,
        LadderBottom
    }
    public BoxStatus status;

    [SerializeField]
    Sprite[] snakeHeadImg;
    [SerializeField]
    Sprite[] snakeTailImg;
    [SerializeField]
    Sprite[] ladderImg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangeStatus(BoxStatus newStatus)
    {
        status = newStatus;

        // Add additional logic or functionality based on the new status
        switch (newStatus)
        {
            case BoxStatus.Init:
                // Perform initialization tasks
                break;
            case BoxStatus.Question:
                // Perform initialization tasks
                break;
            case BoxStatus.BattleQuestion:
                // Perform initialization tasks
                break;
            case BoxStatus.MiniGame:
                // Perform initialization tasks
                break;
            case BoxStatus.BattleMiniGame:
                // Perform initialization tasks
                break;
            case BoxStatus.SnakeHead:
                // Perform initialization tasks
                break;
            case BoxStatus.SnakeTail:
                // Perform initialization tasks
                break;
            case BoxStatus.LadderTop:
                // Perform initialization tasks
                break;
            case BoxStatus.LadderBottom:
                // Perform initialization tasks
                break;
        }
    }

    public void ChangeSnakeHeadIcon(int i)
    {
        gameObject.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = snakeHeadImg[i];
        gameObject.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
    }

    public void ChangeSnakeTailIcon(int i)
    {
        gameObject.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = snakeTailImg[i];
        gameObject.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
    }

    public void ChangeLadderIcon(int i)
    {
        gameObject.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = ladderImg[i];
        gameObject.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
