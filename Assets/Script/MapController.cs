using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    public GameObject map;
    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i< map.transform.childCount; i++)
        {
            if(i%2 == 1)
            {
                map.transform.GetChild(i).GetComponent<Renderer>().material.color = Color.green;
            }
        }

        RanMap();
    }

    public void RanMap()
    {
        int i = 0; 
            i = Random.Range(0, 3);
        
        GameManager.Instance.ChangeTypeMap(i);
        int numQues = UnityEngine.Random.Range(GameManager.Instance.minQues, GameManager.Instance.maxQues +1);
        int numQuesBat = UnityEngine.Random.Range(GameManager.Instance.minQuesBat, GameManager.Instance.maxQuesBat + 1);
        int numMini = UnityEngine.Random.Range(GameManager.Instance.minMini, GameManager.Instance.maxMini + 1);
        int numMiniBat = UnityEngine.Random.Range(GameManager.Instance.minMiniBat, GameManager.Instance.maxMiniBat + 1);
        int numNor = GameManager.Instance.totalMap - numQues - numQuesBat - numMini - numMiniBat - GameManager.Instance.numSnake*2 - GameManager.Instance.numLadder*2- GameManager.Instance.numdef;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
