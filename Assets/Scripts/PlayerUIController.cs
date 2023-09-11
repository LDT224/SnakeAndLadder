using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : Photon.MonoBehaviour
{
    // Start is called before the first frame update
    private LobbyController lobbyController;
    private GameObject parent;
    void Start()
    {
        lobbyController = GameObject.FindObjectOfType<LobbyController>();
        parent = lobbyController.playerList;
        if (photonView.IsMine)
        {
            // ?i?u khi?n c?c b? trên máy ng??i ch?i hi?n t?i
            // Thay ??i v? trí, cha m?, t? l? và các thông s? khác ? ?ây
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            // C?p nh?t v? trí, cha m?, t? l? và các thông s? khác d?a trên d? li?u ??ng b? hóa
            transform.SetParent(parent.transform);
            transform.localScale = Vector3.one;
            
            for(int i = 0; i <parent.transform.childCount; i++)
            {
                if (i==0)
                {
                    parent.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = new Color32(98, 158, 242, 255);
                    parent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "Player1";
                }
                else if (i == 1)
                {
                    parent.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = new Color32(245, 219, 74, 255);
                    parent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "Player2";
                }
                else if (i == 2)
                {
                    parent.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = new Color32(227, 45, 37, 255);
                    parent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "Player3";
                }
                else
                {
                    parent.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = new Color32(58, 172, 81, 255);
                    parent.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "Player4";
                }
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // G?i d? li?u ??ng b? hóa t? máy ch? ??n các máy ch?i khác
            stream.SendNext(transform.parent);
            stream.SendNext(transform.localScale);
            stream.SendNext(transform.GetChild(2).GetComponent<Image>().color);
            stream.SendNext(transform.GetChild(1).GetComponent<Text>().color);
        }
        else
        {
            // Nh?n d? li?u ??ng b? hóa t? máy ch? và c?p nh?t nó trên máy c?a ng??i ch?i
        }
    }
}
