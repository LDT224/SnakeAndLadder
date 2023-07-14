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
    
    private int numQues, numQuesBat, numMini, numMiniBat, numNor;

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
            }
        }
        ChangeMapInfo();
        RandomMap();
    }

    private void ChangeMapInfo()
    {
        int i = 0; 
        i = Random.Range(0, 3);
        GameManager.Instance.ChangeTypeMap(i);

        numQues = UnityEngine.Random.Range(GameManager.Instance.minQues, GameManager.Instance.maxQues +1);
        numQuesBat = UnityEngine.Random.Range(GameManager.Instance.minQuesBat, GameManager.Instance.maxQuesBat + 1);
        numMini = UnityEngine.Random.Range(GameManager.Instance.minMini, GameManager.Instance.maxMini + 1);
        numMiniBat = UnityEngine.Random.Range(GameManager.Instance.minMiniBat, GameManager.Instance.maxMiniBat + 1);
        numNor = GameManager.Instance.totalMap - numQues - numQuesBat - numMini - numMiniBat - GameManager.Instance.numSnake*2 - GameManager.Instance.numLadder*2- GameManager.Instance.numdef;

        int j = 0;
        j = Random.Range(0, 1);
        GameManager.Instance.ChangeSnakeAndLadder(j);
    }

    private void RandomMap()
    {
        temp.AddRange(GameManager.Instance.snakeHead);
        temp.AddRange(GameManager.Instance.snakeTail);
        temp.AddRange(GameManager.Instance.ladderTop);
        temp.AddRange(GameManager.Instance.ladderBot);

        //Random question box
        for(int i = 0; i < numQues; i++)
        {
            int j = Random.Range(1, GameManager.Instance.totalMap);
            if (temp.Contains(j))
                i--;
            else
            {
                temp.Add(j);
                boxs[j].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.Question);
                boxs[j].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "Question";
            }
        }

        //Random question battle box
        for (int i = 0; i < numQuesBat; i++)
        {
            int j = Random.Range(1, GameManager.Instance.totalMap);
            if (temp.Contains(j))
                i--;
            else
            {
                temp.Add(j);
                boxs[j].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.BattleQuestion);
                boxs[j].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "BattleQuestion";
            }
        }

        //Random question box
        for (int i = 0; i < numMini; i++)
        {
            int j = Random.Range(1, GameManager.Instance.totalMap);
            if (temp.Contains(j))
                i--;
            else
            {
                temp.Add(j);
                boxs[j].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.MiniGame);
                boxs[j].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "MiniGame";
            }
        }

        //Random question box
        for (int i = 0; i < numQues; i++)
        {
            int j = Random.Range(1, GameManager.Instance.totalMap);
            if (temp.Contains(j))
                i--;
            else
            {
                temp.Add(j);
                boxs[j].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.BattleMiniGame);
                boxs[j].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "BattleMiniGame";
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
