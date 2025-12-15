using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionScript : MonoBehaviour
{
    public gameLogicScript gameLogic;



    // Start is called before the first frame update
    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<gameLogicScript>();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.layer == 3) {
            gameLogic.gameScore();

        }

    }

}
