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
    }

    [PunRPC]
    void RPC_RollAnimation(int temp)
    {
        AudioManager.instance.PlaySFX("Dice");
        roll = temp;
        Animator animator = GetComponent<Animator>();
        animator.Play("Dice Roll", -1, 0f);
    }

    public void Roll(int temp)
    {
        photonView.RPC("RPC_RollAnimation", RpcTarget.All, temp);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
