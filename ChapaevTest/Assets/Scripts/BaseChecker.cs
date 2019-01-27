using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseChecker : MonoBehaviour {

    GameObject board;
    protected int type = 1; // 0 - bot checker, 1 - player checker
    protected bool isActive = false;

    protected void Start ()
    {
        board = GameObject.FindGameObjectWithTag("Board").gameObject;
    }

    protected void CheckIfCheckerOutOfBoard()
    {
        
        if (!board.GetComponent<BoxCollider2D>().OverlapPoint(this.transform.position))
        {
            
            Destroy(this.gameObject);

            FindObjectOfType<GameManager>().ChangeCheckersCount(type);
            if (isActive)
            {
                FindObjectOfType<GameManager>().NextTurn();
            }

        }
    }
}
