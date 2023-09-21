using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DiceController : MonoBehaviour
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
    public void Roll(int temp)
    {
        roll = temp;
        Animator animator = GetComponent<Animator>();
        animator.Play("Dice Roll", -1, 0f);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
