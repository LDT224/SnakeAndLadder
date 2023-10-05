using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun.Demo.Cockpit;
using System.Linq;

public class LobbyController : MonoBehaviourPunCallbacks
{
    public static LobbyController Instance;

    [SerializeField]
    private InputField roomName;
    [SerializeField]
    private GameObject homePanel;
    [SerializeField]
    private GameObject inRoomPanel;
    [SerializeField]
    private Text roomNameText;
    [SerializeField]
    private GameObject roomList;
    [SerializeField]
    private GameObject roomPrefab;

    public GameObject playerList;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject enterUsernamePanel;
    [SerializeField]
    private InputField userNameText;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connect to server");

        //if(PlayfabController.instance.userName == null)
        //{
        //    enterUsernamePanel.SetActive(true);
        //}
        //Debug.Log("Username: " + PlayerPrefs.GetString("UserName"));

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected to server");
    }

    public override void OnJoinedLobby()
    {
        SetPlayerName();
        Debug.Log("Joined Lobby");
    }
    public void SetPlayerName()
    {
        //Get player name from Playfab
        PhotonNetwork.NickName = PlayerPrefs.GetString("UserName");

    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomName.text))
            return;
        PhotonNetwork.CreateRoom(roomName.text);
    }

    public void JoinRoomByName()
    {
        PhotonNetwork.JoinRoom(roomName.text);   
    }
    public void JoinRoomInList(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
    }

    public override void OnJoinedRoom()
    {
        homePanel.SetActive(false);
        inRoomPanel.SetActive(true);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerPrefab, playerList.transform).GetComponent<PlayerListItem>().SetUp(players[i]);

            if (i == 0)
            {
                playerList.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = new Color32(98, 158, 242, 255);
            }
            else if (i == 1)
            {
                playerList.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = new Color32(245, 219, 74, 255);
            }
            else if (i == 2)
            {
                playerList.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = new Color32(227, 45, 37, 255);
            }
            else
            {
                playerList.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = new Color32(58, 172, 81, 255);
            }
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        homePanel.SetActive(true);
        inRoomPanel.SetActive(false);
        Debug.Log("Left room");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomlist)
    {
        foreach(Transform trans in roomList.transform)
        {
            Destroy(trans.gameObject);
        }

        for(int i =0; i < roomlist.Count; i++)
        {
            Instantiate(roomPrefab, roomList.transform).GetComponent<RoomListItem>().Setup(roomlist[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerPrefab,playerList.transform).GetComponent<PlayerListItem>().SetUp(newPlayer);

        for (int i = 0; i < playerList.transform.childCount; i++)
        {
            if (i == 0)
            {
                playerList.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = new Color32(98, 158, 242, 255);
            }
            else if (i == 1)
            {
                playerList.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = new Color32(245, 219, 74, 255);
            }
            else if (i == 2)
            {
                playerList.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = new Color32(227, 45, 37, 255);
            }
            else
            {
                playerList.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = new Color32(58, 172, 81, 255);
            }
        }
    }

    public void SubmitUsername()
    {
        PlayfabController.instance.UpdateUserName(userNameText.text);
        SetPlayerName();
    }
    public void StartGame()
    {
        //PhotonNetwork.LoadLevel("Gameplay");
    }
    // Update is called once per frame
    void Update()
    {
    }
}
