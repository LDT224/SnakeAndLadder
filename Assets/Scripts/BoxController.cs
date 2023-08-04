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

    public void CheckSlotPlayer(GameObject player)
    {
        if (gameObject.transform.GetChild(2).GetChild(0).childCount == 0)
        {
            player.transform.parent = gameObject.transform.GetChild(2).GetChild(0);

        }
        else if(gameObject.transform.GetChild(2).GetChild(1).childCount == 0)
        {
            gameObject.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            player.transform.parent = gameObject.transform.GetChild(2).GetChild(1);
        }
        else if (gameObject.transform.GetChild(2).GetChild(2).childCount == 0)
        {
            gameObject.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
            player.transform.parent = gameObject.transform.GetChild(2).GetChild(2);
        }
        else
        {
            gameObject.transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
            player.transform.parent = gameObject.transform.GetChild(2).GetChild(3);
        }
        player.transform.localPosition = Vector3.zero;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
