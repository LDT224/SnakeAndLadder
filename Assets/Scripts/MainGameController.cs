using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainGameController : MonoBehaviour
{
    public int numRoll;
    public int currentPlayer = 0;
    private List<GameObject> players = new List<GameObject>();
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    Color[] playerColor;
    private int numPlayer;
    private int[] currentPos = new int[] { 0, 0, 0, 0 };
    public MapController mapController;
    private List<GameObject> maps = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ChangeStatus(GameManager.GameStatus.Play);

        mapController = FindObjectOfType<MapController>();
        maps = mapController.boxs;

        numPlayer = GameManager.Instance.numPlayer;
        SpawnPlayer();
        SetCurrentPlayer();
    }

    public void SpawnPlayer()
    {
        for (int i = 0; i < numPlayer; i++)
        {
            GameObject player = Instantiate(playerPrefab,Vector3.zero, Quaternion.identity);
            players.Add(player);
            maps[0].GetComponent<BoxController>().CheckSlotPlayer(players[i]);
            player.GetComponent<SpriteRenderer>().color = playerColor[i];
        }
    }
    public void SetCurrentPlayer()
    {
    }

    public void NextTurn()
    {
        currentPlayer = (currentPlayer + 1) % players.Count;
        SetCurrentPlayer();
    }

    public void PlayerMove(int currentPlayer,int numRoll)
    {
        int newPos = currentPos[currentPlayer] + numRoll;
        if(newPos < 34)
        {
            currentPos[currentPlayer] = newPos;
            maps[newPos].GetComponent<BoxController>().CheckSlotPlayer(players[currentPlayer]);
            Debug.Log("Player " + currentPlayer + "move to " + currentPos[currentPlayer]);
        }
        else if(newPos == 34)
        {
            currentPos[currentPlayer] = newPos;
            maps[newPos].GetComponent<BoxController>().CheckSlotPlayer(players[currentPlayer]);
            Debug.Log("Player: " + currentPlayer + "WINNNNN!!!!");
        }
        else
        {
            Debug.Log("Over map =>> reRoll");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.canRoll == false) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousepos2d = new Vector2(mousepos.x, mousepos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousepos2d, Vector2.zero);

            if (!hit.collider) return;

            if (hit.collider.CompareTag("Dice"))
            {
                numRoll = 1 + Random.Range(0, 6);
                hit.collider.gameObject.GetComponent<DiceController>().Roll(numRoll);
                GameManager.Instance.ChangeStatus(GameManager.GameStatus.InTurn);
                Debug.Log( "roll: " + numRoll);
            }
        }
    }
}
