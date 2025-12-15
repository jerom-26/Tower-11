using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerExplosionHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public gameLogicScript gameLogic;
    public GameObject explosionEffect;
    private bool moveStop = false;

    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<gameLogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Debug.Log("Explosion triggered!");
            towerPause();
            stopTowerSpan();
            Destroy(gameObject);
            gameLogic.gameOverScreen();
            
        }
    }

   
    void towerPause()
    {

        if(!moveStop)
        {
            towerMoveScript towerMovement = GetComponent<towerMoveScript>();

            if (towerMovement != null) { 

                towerMovement.collisionMoveStop = false;

            }
            
        }
        
    }

    void stopTowerSpan()
    {
        towerSpawnScript towerSpawnn = GameObject.FindAnyObjectByType<towerSpawnScript>();

        if (towerSpawnn != null)
        {
            towerSpawnn.collisionSpawnStop = false;
        }
    }
}
