using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField]
    private Text roomNameTxt;

    public RoomInfo info;

    public void Setup(RoomInfo _info)
    {
        info = _info;
        roomNameTxt.text = _info.Name;
    }

    public void OnClick()
    {
        LobbyController.Instance.JoinRoomInList(info);
    }
}
