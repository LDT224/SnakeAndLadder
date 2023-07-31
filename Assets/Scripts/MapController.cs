using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    [SerializeField]
    GameObject map;
    
    private int numQues, numQuesBat, numMini, numMiniBat, numNor, numSnake, numLadder;

    private List<GameObject> boxs = new List<GameObject>();
    private List<int> temp = new List<int> ();
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < GameManager.Instance.totalMap; i++)
        {
            for(int j = 0; j < map.transform.childCount; j++)
            {
                if(map.transform.GetChild(j).name == (i+1).ToString())
                {
                    boxs.Add(map.transform.GetChild(j).GameObject());
                }

                //map.transform.GetChild(j).GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        ChangeMapInfo();
    }

    private void ChangeMapInfo()
    {
        int i = 0; 
        i = Random.Range(0, 3);
        GameManager.Instance.ChangeTypeMap(i);

        numSnake = GameManager.Instance.numSnake;
        numLadder = GameManager.Instance.numLadder;
        numQues = UnityEngine.Random.Range(GameManager.Instance.minQues, GameManager.Instance.maxQues +1);
        numQuesBat = UnityEngine.Random.Range(GameManager.Instance.minQuesBat, GameManager.Instance.maxQuesBat + 1);
        numMini = UnityEngine.Random.Range(GameManager.Instance.minMini, GameManager.Instance.maxMini + 1);
        numMiniBat = UnityEngine.Random.Range(GameManager.Instance.minMiniBat, GameManager.Instance.maxMiniBat + 1);
        numNor = GameManager.Instance.totalMap - numQues - numQuesBat - numMini - numMiniBat - GameManager.Instance.numSnake*2 - GameManager.Instance.numLadder*2- GameManager.Instance.numdef;

        int j = 0;
        j = Random.Range(0, 1);
        GameManager.Instance.ChangeSnakeAndLadder(j);
        RandomMap();
        Debug.Log("Num snake: " + numSnake + ", num ladder: " + numLadder + ", num ques: " + numQues + ", num quesbat: " + numQuesBat + ", num mini: " + numMini + ", num minibat: " + numMiniBat);
    }

    private void RandomMap()
    {
        //temp.AddRange(GameManager.Instance.snakeHead);
        //temp.AddRange(GameManager.Instance.snakeTail);
        //temp.AddRange(GameManager.Instance.ladderTop);
        //temp.AddRange(GameManager.Instance.ladderBot);

        //Random Snake box
        for(int i =0; i<numSnake; i++)
        {
            //Snake head
            int head = Random.Range(2, GameManager.Instance.totalMap-1);
            if (temp.Contains(head))
            {
                i--;
                continue;
            }
            else
            {
                temp.Add(head);
                boxs[head].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.SnakeHead);
                boxs[head].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "SnakeH" + i;
            }
            //Snake tail
            int tail = Random.Range(1, head);
            if (temp.Contains(tail))
            {
                i--;
                temp.Remove(head);
                continue;
            }
            else
            {
                temp.Add(tail);
                boxs[tail].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.SnakeTail);
                boxs[tail].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "SnakeT" + i;
            }
                
                boxs[head].GetComponent<BoxController>().ChangeSnakeHeadIcon(i);
                boxs[tail].GetComponent<BoxController>().ChangeSnakeTailIcon(i);
        }

        //Random Ladder box
        for (int i = 0; i < numLadder; i++)
        {
            //Ladder bottom
            int bottom = Random.Range(1, 30);
            if (temp.Contains(bottom))
            {
                i--;
                continue;
            }
            else
            {
                temp.Add(bottom);
                boxs[bottom].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.LadderBottom);
                boxs[bottom].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "LaddB" + i;
            }

            //Ladder top
            int top = Random.Range(bottom, GameManager.Instance.totalMap-1);
            if (temp.Contains(top))
            {
                i--;
                temp.Remove(bottom);
                continue;
            }
            else
            {
                temp.Add(top);
                boxs[top].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.LadderTop);
                boxs[top].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "LaddT" + i;
            }

                boxs[top].GetComponent<BoxController>().ChangeLadderIcon(i);
                boxs[bottom].GetComponent<BoxController>().ChangeLadderIcon(i);

        }

        //Random question box
        for (int i = 0; i < numQues; i++)
        {
            int j = Random.Range(1, GameManager.Instance.totalMap - 1);
            if (temp.Contains(j))
                i--;
            else
            {
                temp.Add(j);
                boxs[j].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.Question);
                boxs[j].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "Ques" + i;
            }
        }

        //Random question battle box
        for (int i = 0; i < numQuesBat; i++)
        {
            int j = Random.Range(1, GameManager.Instance.totalMap - 1);
            if (temp.Contains(j))
                i--;
            else
            {
                temp.Add(j);
                boxs[j].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.BattleQuestion);
                boxs[j].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "BQues" + i;
            }
        }

        //Random question box
        for (int i = 0; i < numMini; i++)
        {
            int j = Random.Range(1, GameManager.Instance.totalMap - 1);
            if (temp.Contains(j))
                i--;
            else
            {
                temp.Add(j);
                boxs[j].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.MiniGame);
                boxs[j].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "MiniG" + i;
            }
        }

        //Random question box
        for (int i = 0; i < numMiniBat; i++)
        {
            int j = Random.Range(1, GameManager.Instance.totalMap - 1);
            if (temp.Contains(j))
                i--;
            else
            {
                temp.Add(j);
                boxs[j].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.BattleMiniGame);
                boxs[j].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "BMiniG" + i;
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
