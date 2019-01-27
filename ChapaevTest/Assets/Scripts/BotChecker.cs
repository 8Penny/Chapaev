using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotChecker : BaseChecker {

    Vector2 direction, newDirection;
    GameObject arrow;
    bool isDone = false;
    bool isAiming = false;
    float distance = 0.3f;
    int sideArrowShift = 1; // 1 - right side, -1 - left side
    


    new void Start ()
    {
        base.Start();
        arrow = this.gameObject.transform.GetChild(0).gameObject; // arrow shows force
        base.type = 0;
    }
	

	void Update ()
    {
        base.CheckIfCheckerOutOfBoard();
        if (isDone & this.GetComponent<Rigidbody2D>().velocity == new Vector2(0, 0))
        {
            FindObjectOfType<GameManager>().NextTurn();
            isDone = false;
            base.isActive = false;
        }
        if (isAiming)
        {
            if(newDirection!=direction)
            {
                Aiming();
            }
            else
            {
                this.GetComponent<Rigidbody2D>().velocity = direction * 16;
                isDone = true;
                isAiming = false;
                arrow.transform.localScale = new Vector3(0.3f, 0.1f, 0); // hide arrow
            }
            
        }
    }

    public void MakeMove(Vector2 d)
    {
        base.isActive = true;
        float force = Random.Range(1.0f, 1.5f);
        direction = Vector2.ClampMagnitude(d, force);
        int degrees = Random.Range(10, 40);
        sideArrowShift = Random.Range(0, 2) * 2 - 1;
        newDirection = RotateDirectionVector(direction, sideArrowShift*degrees);
        isAiming = true;
        
    }

    void Aiming()
    {

        if (distance < direction.magnitude)
        { distance += 0.05f; }
        float angle = Mathf.Atan2(newDirection.x, newDirection.y) * Mathf.Rad2Deg; // arrow rotation angle
        arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -angle)); // rotate arrow
        arrow.transform.localScale = new Vector3(0.3f, distance, 0); // change arrow length
        newDirection = RotateDirectionVector(newDirection, -1f * sideArrowShift);

    }

    Vector2 RotateDirectionVector(Vector2 v, float degrees)
    {
         float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
         float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

         float tx = v.x;
         float ty = v.y;
         v.x = (cos * tx) - (sin * ty);
         v.y = (sin * tx) + (cos * ty);
         return v;

    }
}
