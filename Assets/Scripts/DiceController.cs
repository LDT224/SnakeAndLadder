using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DiceController : MonoBehaviourPunCallbacks
{
    private int roll;
    [SerializeField]
    List<Sprite> dice;

    public MainGameController mainGameController;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void RandomImage()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = dice[Random.Range(0, dice.Count)];
    }
    
    public void SetImage()
    {
        SpriteRenderer render = GetComponent<SpriteRenderer>();
        render.sprite = dice[roll-1];

        //Roll dice available
        mainGameController.PlayerMove(mainGameController.currentPlayer, mainGameController.numRoll);
    }

    [PunRPC]
    void RPC_RollAnimation()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("Dice Roll", -1, 0f);
    }

    public void Roll(int temp)
    {
        roll = temp;

        photonView.RPC("RPC_RollAnimation", RpcTarget.All);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
