using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerSpawnScript : MonoBehaviour
{
    public GameObject towerPrefab;
    public float spawnRate = 2f;

    private float timer = 0f;
    public bool collisionSpawnStop = true;

    public float delay = 2f;
    private bool canSpawnTowers = false;

    void Start()
    {
        canSpawnTowers = false;
        timer = 0f;

        Invoke(nameof(EnableTowerSpawning), delay);
    }

    void EnableTowerSpawning()
    {
        canSpawnTowers = true;
        timer = 0f;
    }

    void Update()
    {
        if (!canSpawnTowers) return;
        if (!collisionSpawnStop) // stop spawning if false
            return;

        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            Spawntower();
            timer = 0f;
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
