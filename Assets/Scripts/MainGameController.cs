using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainGameController : MonoBehaviour
{
    public int numRoll;
    private int currentPlayer = 0;
    private List<GameObject> players = new List<GameObject>();
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    GameObject startBox;
    [SerializeField]
    Color[] playerColor;
    private int numPlayer;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ChangeStatus(GameManager.GameStatus.Play);
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
            startBox.GetComponent<BoxController>().CheckSlotPlayer(player);
            player.GetComponent<SpriteRenderer>().color = playerColor[i];
            Debug.Log(GameManager.Instance.numPlayer);
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
