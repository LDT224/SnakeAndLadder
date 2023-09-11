using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] 
    private string VersionName = "0.1";
    [SerializeField]
    private InputField roomName;
    [SerializeField]
    private GameObject homePanel;
    [SerializeField]
    private GameObject inRoomPanel;

    [SerializeField]
    private GameObject playerList;
    [SerializeField]
    private GameObject player;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(VersionName);
    }
    void Start()
    {
        
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connect to server");
    }

    public void SetPlayerName()
    {
        //Get player name from Playfab
        PhotonNetwork.playerName = "";

    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomName.text, new RoomOptions() { maxPlayers = 4}, null);
    }

    public void JoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default);
    }

    private void OnJoinedRoom()
    {
        homePanel.SetActive(false);
        inRoomPanel.SetActive(true);
        
        GameObject playerInRoom = Instantiate(player, Vector3.zero, Quaternion.identity);
        playerInRoom.transform.parent = playerList.transform;
        playerInRoom.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = null; // Get player avatar
        playerInRoom.transform.GetChild(1).GetComponent<Text>().text = PhotonNetwork.playerName;
        playerInRoom.transform.localScale = Vector3.one;

        int num = playerList.transform.childCount;
        switch (num)
        {
            case 1:
                playerInRoom.transform.GetChild(2).GetComponent<Image>().color = new Color32(98, 158, 242, 255);
                break;
            case 2:
                playerInRoom.transform.GetChild(2).GetComponent<Image>().color = new Color32(245, 219, 74, 255);
                break;
            case 3:
                playerInRoom.transform.GetChild(2).GetComponent<Image>().color = new Color32(227, 45, 37, 255);
                break;
            case 4:
                playerInRoom.transform.GetChild(2).GetComponent<Image>().color = new Color32(58, 172, 81, 255);
                break;

        };
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Gameplay");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
