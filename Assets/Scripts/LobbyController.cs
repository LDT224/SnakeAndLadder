using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField roomName;
    [SerializeField]
    private GameObject homePanel;
    [SerializeField]
    private GameObject inRoomPanel;

    public GameObject playerList;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enterUsernamePanel;
    [SerializeField]
    private InputField userNameText;
    [SerializeField]
    private Text roomNameText;
    
    // Start is called before the first frame update
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

    public void JoinRoom()
    {
       // RoomOptions roomOptions = new RoomOptions();
       // roomOptions.maxPlayers = 4;
       // PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        SetPlayerName();
        homePanel.SetActive(false);
        inRoomPanel.SetActive(true);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        //SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        //GameObject playerInRoom = PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity, 0);

        //playerInRoom.transform.SetParent(playerList.transform);
        //playerInRoom.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = null; // Get player avatar
        //playerInRoom.transform.localScale = Vector3.one;
        //playerInRoom.transform.GetChild(2).GetComponent<Image>().color = new Color32(98, 158, 242, 255);
        //playerInRoom.transform.GetChild(1).GetComponent<Text>().text = PhotonNetwork.playerName;
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
    public void SubmitUsername()
    {
        PlayfabController.instance.UpdateUserName(userNameText.text);
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
