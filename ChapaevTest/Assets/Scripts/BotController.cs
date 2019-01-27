using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour {

    public void MakeMove()
    { 
        // choose aim
        GameObject[] playerCheckers = GameObject.FindGameObjectsWithTag("PlayerChecker");
        GameObject aim = playerCheckers[Random.Range(0, playerCheckers.Length)];
        // find close colliders
        Collider2D[] colliders = Physics2D.OverlapCircleAll(aim.transform.position, 1f,LayerMask.GetMask("PlayerCheckers"));
        GameObject secondAim = null;
        foreach (Collider2D c in colliders)
        {
            if (c.gameObject != aim)
            { secondAim = c.gameObject; } // choose second aim
        }
        // choose active checker
        GameObject[] botCheckers = GameObject.FindGameObjectsWithTag("BotChecker");
        int activeInd = 0;
        for (int ind = 1; ind < botCheckers.Length; ind++) // choose closet active checker to aim
        {
            if (Vector2.Distance(botCheckers[activeInd].transform.position,aim.transform.position) > Vector2.Distance(botCheckers[ind].transform.position, aim.transform.position))
            {
                activeInd = ind;
            }
        }
        GameObject activeChecker = botCheckers[activeInd];

        // choose direction: if second aim choose middle point between aims
        Vector2 direction = (secondAim != null)? Vector3.Lerp(aim.transform.position, secondAim.transform.position, 0.5f) - activeChecker.transform.position : aim.transform.position - activeChecker.transform.position;
        // make move
        activeChecker.GetComponent<BotChecker>().MakeMove(direction);

    }
}
