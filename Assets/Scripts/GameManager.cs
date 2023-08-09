using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameStatus
    {
        Init,
        Play,
        InTurn,
        InQuestion,
        InBattleQuestion,
        InMiniGame,
        InBattleMiniGame,
        Finish
    }
    public GameStatus status;

    // Singleton instance
    private static GameManager instance;

    //Map information
    public int numPlayer = 4;
    public int totalMap = 35;
    public int numdef =2;
    public int numSnake;
    public int numLadder;
    public int numNor;
    
    public int minQues;
    public int maxQues;
    
    public int minQuesBat;
    public int maxQuesBat;
    
    public int minMini;
    public int maxMini;

    public int minMiniBat;
    public int maxMiniBat;

    public List<int> snakeHead = new List<int>();
    public List<int> snakeTail = new List<int>();
    public List<int> ladderTop = new List<int>();
    public List<int> ladderBot = new List<int>();
    //Dice
    public bool canRoll;
    //public int numRoll;
    // Public accessor for the singleton instance
    public static GameManager Instance
    {
        get { return instance; }
    }
    //Change the game status
    public void ChangeStatus(GameStatus newStatus)
    {
        status = newStatus;

        // Add additional logic or functionality based on the new status
        switch (newStatus)
        {
            case GameStatus.Init:
                canRoll = false;
                // Perform initialization tasks
                break;
            case GameStatus.Play:
                canRoll = true;
                // Start gameplay
                break;
            case GameStatus.InTurn:
                canRoll = false;
                // Start gameplay
                break;
            case GameStatus.InQuestion:
                // Start gameplay
                canRoll = false;

                break;
            case GameStatus.InBattleQuestion:
                canRoll = false;
                // Start gameplay
                break;
            case GameStatus.InMiniGame:
                canRoll = false;
                // Start gameplay
                break;
            case GameStatus.InBattleMiniGame:
                canRoll = false;
                // Start gameplay
                break;
            case GameStatus.Finish:
                canRoll = false;
                // Handle game over or player death
                break;
        }
    }
    //Change map information
    public void ChangeTypeMap(int type)
    {
        switch (type)
        {
            //case 3 snake 3 ladder
            case 0:
                numSnake = 3;
                numLadder = 3;
                minQues = 4;
                maxQues = 5;
                minQuesBat = 2;
                maxQuesBat = 3;
                minMini = 4;
                maxMini = 5;
                minMiniBat = 2;
                maxMiniBat = 3;
                break;

            //case 2 snake 3 ladder
            case 1:
                numSnake = 2;
                numLadder = 3;
                minQues = 4;
                maxQues = 5;
                minQuesBat = 3;
                maxQuesBat = 4;
                minMini = 4;
                maxMini = 5;
                minMiniBat = 3;
                maxMiniBat = 4;
                break;
            //case 3 snake 2 ladder
            case 2:
                numSnake = 2;
                numLadder = 3;
                minQues = 4;
                maxQues = 5;
                minQuesBat = 3;
                maxQuesBat = 4;
                minMini = 4;
                maxMini = 5;
                minMiniBat = 3;
                maxMiniBat = 4;
                break;
        }
    }

    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            // Destroy duplicate instance
            Destroy(gameObject);
            return;
        }

        // Set the instance
        instance = this;

        //Can roll dice
        //canRoll = true;

        // Keep the GameManager object throughout scenes
        DontDestroyOnLoad(gameObject);

    }

    private void OnDestroy()
    {
        // Clear the instance if it's being destroyed
        if (instance == this)
        {
            instance = null;
        }
    }

}
