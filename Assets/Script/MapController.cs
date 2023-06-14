using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
