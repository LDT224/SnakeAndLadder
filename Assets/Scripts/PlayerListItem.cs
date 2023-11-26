using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text playerNameTxt;
    [SerializeField]
    private Text playerScoreTxt;
    [SerializeField]
    private Text playerRankTxt;
    Player player;

    private LobbyController lobbyController;
    private GameObject parent;

    private void Start()
    {
        lobbyController = GameObject.FindObjectOfType<LobbyController>();
        parent = lobbyController.playerList;
    }
    public void SetUp(Player _player)
    {
        player = _player;

        playerNameTxt.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
