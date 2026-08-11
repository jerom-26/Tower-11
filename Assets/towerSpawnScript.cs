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

    [Header("Difficulty Scaling")]
    [SerializeField] private float easySpawnRate = 2f;
    [SerializeField] private float hardSpawnRate = 0.9f;

    [Header("References")]
    [SerializeField] private gameLogicScript gameLogic;

    private float timer = 0f;
    private bool canSpawnTowers = false;
    void Start()
    {
        if (gameLogic == null)
        {
            GameObject logicObject =
                GameObject.FindGameObjectWithTag("Logic");

            if (logicObject != null)
            {
                gameLogic =
                    logicObject.GetComponent<gameLogicScript>();
            }
        }

        timer = 0f;
        canSpawnTowers = false;
        collisionSpawnStop = true;

        Invoke(nameof(EnableTowerSpawning), delay);
    }

    private void EnableTowerSpawning()
    {
        if (gameLogic == null || !gameLogic.IsPlaying)
        {
            return;
        }

        canSpawnTowers = true;
        timer = 0f;
    }

    private void Update()
    {
        if (gameLogic == null || !gameLogic.IsPlaying)
        {
            return;
        }

        if (!canSpawnTowers || !collisionSpawnStop)
        {
            return;
        }

        timer += Time.deltaTime;

        float difficulty =
            gameLogic.GetDifficulty01();

        float currentSpawnRate = Mathf.Lerp(
            easySpawnRate,
            hardSpawnRate,
            difficulty
        );

        if (timer >= currentSpawnRate)
        {
            SpawnTower();
            timer = 0f;
        }

    }

    private void SpawnTower()
    {
        if (towerPool == null)
        {
            Debug.LogError(
                "Tower Pool is not assigned.",
                this
            );

            return;
        }

        GameObject towerObject = towerPool.Get(
            transform.position,
            transform.rotation
        );

        if (towerObject == null)
        {
            return;
        }

        PooledObject pooled =
            towerObject.GetComponent<PooledObject>();

        if (pooled != null)
        {
            pooled.SetPool(towerPool);
        }

        TowerBehaviour tower =
            towerObject.GetComponent<TowerBehaviour>();

        if (tower != null)
        {
            tower.OnSpawned();
        }
    }

    public void StopCollisionSpawning()
    {
        collisionSpawnStop = false;
        canSpawnTowers = false;
        timer = 0f;

        CancelInvoke(nameof(EnableTowerSpawning));
    }
    private void OnDisable()
    {
        CancelInvoke(nameof(EnableTowerSpawning));
    }
}
