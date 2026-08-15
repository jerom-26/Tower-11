using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionScript : MonoBehaviour
{
    public gameLogicScript gameLogic;

    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<gameLogicScript>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.layer == 3) {
            gameLogic.gameScore();

        }

    }

}
