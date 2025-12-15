using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerSpawnScript : MonoBehaviour
{
    public GameObject towerPrefab;
    public float spawnRate = 2f;
    private float timer;
    public bool collisionSpawnStop = true; // true = allow spawning

    void Start()
    {
        Spawntower();
    }

    void Update()
    {
        if (!collisionSpawnStop) // stop spawning if false
            return;

        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Spawntower();
            timer = 0;
        }
    }

    void Spawntower()
    {
        GameObject newTower = Instantiate(towerPrefab, transform.position, transform.rotation);

        // Pick random scale in Y
        float randomHeight = Random.Range(.8f , 1f); // values
        newTower.transform.localScale = new Vector3(
            newTower.transform.localScale.x,
            randomHeight,
            newTower.transform.localScale.z
        );
    }

    // Call this when game is over
    public void StopCollisionSpawning()
    {
        collisionSpawnStop = false;
    }
}
