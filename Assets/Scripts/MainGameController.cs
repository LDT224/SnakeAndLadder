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
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ChangeStatus(GameManager.GameStatus.Play);
        for(int i = 0; i < 4; i++)
        {
            Vector3 spawnPos = new Vector3(0,0,0);
            GameObject player = Instantiate(playerPrefab,spawnPos,Quaternion.identity);
            players.Add(player);
            player.transform.parent = startBox.transform.GetChild(2);
            player.transform.localPosition = spawnPos;
            player.GetComponent<SpriteRenderer>().color = playerColor[i];
        }
        SetCurrentPlayer();
    }

    public void SetCurrentPlayer()
    {
        players[currentPlayer].GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    public void NextTurn()
    {
        players[currentPlayer].GetComponent<SpriteRenderer>().sortingOrder = 2;
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
