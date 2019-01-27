using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerChecker : BaseChecker {

    bool isAiming = false; // is player aiming
    Vector3 direction;
    GameObject arrow;
    bool isDone = false; //is move done?

    new void Start ()
    {
        base.Start();
        arrow = this.gameObject.transform.GetChild(0).gameObject; // arrow shows force
    }

	void Update ()
    {
        base.CheckIfCheckerOutOfBoard();
        if (!FindObjectOfType<GameManager>().blockCheckers)
        {
            CheckerLoop();
        }
        if (isDone & this.GetComponent<Rigidbody2D>().velocity == new Vector2(0,0))
        {
            FindObjectOfType<GameManager>().NextTurn();
            isDone = false;
            base.isActive = false;
        }
    }

    void CheckerLoop()
    {   
        if (!isAiming)
        { CheckIfClick(); }
        else
        { Aiming(); }

    }
    void CheckIfClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //get the point on screen user has tapped
            Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //if user has tapped onto the checker

            if (this.GetComponent<CircleCollider2D>().OverlapPoint(location))
            {
                isAiming = true;
            }
        }
    }

    void Aiming()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 aimPos = this.gameObject.transform.position;
            location.z = aimPos.z = 0;
           
            direction = location - aimPos;
            direction = (direction.magnitude > 1.5f) ? Vector3.ClampMagnitude(direction, 1.5f):direction; // create a power limit
            float distance = Vector2.Distance(location, aimPos);
            distance = (distance > 1.5f) ? 1.5f : distance; // create an arrow length limit

            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg; // arrow rotation angle
            arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -angle)); // rotate arrow
            arrow.transform.localScale = new Vector3(0.3f, distance, 0); // change arrow length

        }
        else//user has removed the tap 
        {
            base.isActive = true;
            this.GetComponent<Rigidbody2D>().velocity = direction * 16;
            arrow.transform.localScale = new Vector3(0.3f, 0.1f, 0); // hide arrow
            isAiming = false;
            isDone = true;
            FindObjectOfType<GameManager>().blockCheckers = true;


        }
    }


}
