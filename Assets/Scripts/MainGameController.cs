using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameController : MonoBehaviour
{
    public int numRoll;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ChangeStatus(GameManager.GameStatus.Play);
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
                Debug.Log( "roll: " + numRoll);
                GameManager.Instance.ChangeStatus(GameManager.GameStatus.InTurn);
            }
        }
    }
}
