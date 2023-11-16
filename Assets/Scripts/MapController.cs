using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject map;
    
    private int numQues, numQuesBat, numMini, numMiniBat, numNor, numSnake, numLadder;

    public List<GameObject> boxs = new List<GameObject>();
    private List<int> temp = new List<int> ();

    private MainGameController gameController;
    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();


    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<MainGameController>();

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
        if (PhotonNetwork.IsMasterClient)
        {
            ChangeMapInfo();

            _myCustomProperties["mapList"] = temp;
            _myCustomProperties["numSnake"] = numSnake;
            _myCustomProperties["numLadder"] = numLadder;
            _myCustomProperties["numQues"] = numQues;
            _myCustomProperties["numQuesBat"] = numQuesBat;
            _myCustomProperties["numMini"] = numMini;
            _myCustomProperties["numMiniBat"] = numMiniBat;
            PhotonNetwork.CurrentRoom.SetCustomProperties(_myCustomProperties);
        }
    }

    public void ChangeMapInfo()
    {
        int i = 0; 
        i = Random.Range(0, 3);
        GameManager.Instance.ChangeTypeMap(3);

        numSnake = GameManager.Instance.numSnake;
        numLadder = GameManager.Instance.numLadder;
        numQues = UnityEngine.Random.Range(GameManager.Instance.minQues, GameManager.Instance.maxQues +1);
        numQuesBat = UnityEngine.Random.Range(GameManager.Instance.minQuesBat, GameManager.Instance.maxQuesBat + 1);
        numMini = UnityEngine.Random.Range(GameManager.Instance.minMini, GameManager.Instance.maxMini + 1);
        numMiniBat = UnityEngine.Random.Range(GameManager.Instance.minMiniBat, GameManager.Instance.maxMiniBat + 1);
        numNor = GameManager.Instance.totalMap - numQues - numQuesBat - numMini - numMiniBat - GameManager.Instance.numSnake*2 - GameManager.Instance.numLadder*2- GameManager.Instance.numdef;

        Debug.Log("Num snake: " + numSnake + ", num ladder: " + numLadder + ", num ques: " + numQues + ", num quesbat: " + numQuesBat + ", num mini: " + numMini + ", num minibat: " + numMiniBat);
        RandomMap();
    }

    public void RandomMap()
    {
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

            gameController.snakes.Add(head, tail);
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

            gameController.ladders.Add(bottom, top);
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

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        if (!PhotonNetwork.IsMasterClient)
        {
            if (propertiesThatChanged.ContainsKey("mapList"))
            {
                List<int> mapList = (List<int>)propertiesThatChanged["mapList"];
                string mapListString = string.Join(", ", mapList.Select(x => x.ToString()).ToArray());
                Debug.Log("Map List: " + mapListString);

                int _numSnake = (int)propertiesThatChanged["numSnake"];
                int _numLadder = (int)propertiesThatChanged["numLadder"];
                int _numQues = (int)propertiesThatChanged["numQues"];
                int _numQuesBat = (int)propertiesThatChanged["numQuesBat"];
                int _numMini = (int)propertiesThatChanged["numMini"];
                int _numMiniBat = (int)propertiesThatChanged["numMiniBat"];

                for (int j = 0; j < _numSnake; j++)
                {
                    boxs[mapList[0]].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.SnakeHead);
                    boxs[mapList[0]].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "SnakeH" + j;

                    boxs[mapList[1]].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.SnakeTail);
                    boxs[mapList[1]].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "SnakeT" + j;

                    boxs[mapList[0]].GetComponent<BoxController>().ChangeSnakeHeadIcon(j);
                    boxs[mapList[1]].GetComponent<BoxController>().ChangeSnakeTailIcon(j);
                    gameController.snakes.Add(mapList[0], mapList[1]);
                    mapList.RemoveAt(1);
                    mapList.RemoveAt(0);
                }

                for (int j = 0; j < _numLadder; j++)
                {
                    boxs[mapList[0]].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.LadderBottom);
                    boxs[mapList[0]].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "LadderB" + j;

                    boxs[mapList[1]].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.LadderTop);
                    boxs[mapList[1]].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "LadderT" + j;

                    boxs[mapList[0]].GetComponent<BoxController>().ChangeLadderIcon(j);
                    boxs[mapList[1]].GetComponent<BoxController>().ChangeLadderIcon(j);
                    gameController.ladders.Add(mapList[0], mapList[1]);
                    mapList.RemoveAt(1);
                    mapList.RemoveAt(0);
                }

                for(int j = 0; j < _numQues; j++)
                {
                    boxs[mapList[0]].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.Question);
                    boxs[mapList[0]].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "Ques" + j;
                    mapList.RemoveAt(0);
                }

                for (int j = 0; j < _numQuesBat; j++)
                {
                    boxs[mapList[0]].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.BattleQuestion);
                    boxs[mapList[0]].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "BQues" + j;
                    mapList.RemoveAt(0);
                }

                for (int j = 0; j < _numMini; j++)
                {
                    boxs[mapList[0]].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.MiniGame);
                    boxs[mapList[0]].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "Mini" + j;
                    mapList.RemoveAt(0);
                }

                for (int j = 0; j < _numMiniBat; j++)
                {
                    boxs[mapList[0]].GetComponent<BoxController>().ChangeStatus(BoxController.BoxStatus.BattleMiniGame);
                    boxs[mapList[0]].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "BMini" + j;
                    mapList.RemoveAt(0);
                }

                string mapListString2 = string.Join(", ", mapList.Select(x => x.ToString()).ToArray());
                Debug.Log("Map List: " + mapListString2);
            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
