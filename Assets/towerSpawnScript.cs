using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerSpawnScript : MonoBehaviour
{
    public bool collisionSpawnStop = true;

    [Header("Pooling")]
    [SerializeField] private ObjectPool towerPool;

    [Header("Spawn Timing")]
    [SerializeField] private float delay = 2f;

    private float timer = 0f;
    private bool canSpawnTowers = false;

    [Header("Difficulty Scaling")]
    [SerializeField] private float easySpawnRate = 2f;
    [SerializeField] private float hardSpawnRate = 0.9f;

    private gameLogicScript gameLogic;
    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<gameLogicScript>();

        timer = 0f;
        canSpawnTowers = false;

        // delay prevents heavy spawn burst right at game start
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

        float difficulty01 = 0f;
        if (gameLogic != null)
            difficulty01 = gameLogic.GetDifficulty01();

        float currentSpawnRate = Mathf.Lerp(easySpawnRate, hardSpawnRate, difficulty01);

        if (timer >= currentSpawnRate)
        {
            Spawntower();
            timer = 0f;
        }

    }


    void Spawntower()
    {
        var obj = towerPool.Get(transform.position, transform.rotation);
        if (obj == null) return;

        // Link object -> pool once, so it can return itself later
        var pooled = obj.GetComponent<PooledObject>();
        if (pooled != null) pooled.SetPool(towerPool);

        //Reset / randomize tower state each time its reused
        var tower = obj.GetComponent<TowerBehaviour>();
        if (tower != null) tower.OnSpawned();
    }

    // Call this when game is over
    public void StopCollisionSpawning()
    {
        collisionSpawnStop = false;
    }
}
