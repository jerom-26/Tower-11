using UnityEngine;

public class towerExplosionHandler : MonoBehaviour
{
    public gameLogicScript gameLogic;

    private ObjectPool explosionPool;
    private PooledObject pooledObject;

    void Awake()
    {
        pooledObject = GetComponent<PooledObject>();
    }

    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag("Logic")
            .GetComponent<gameLogicScript>();

        explosionPool = GameObject.Find("ExplosionPool")
            .GetComponent<ObjectPool>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        SpawnExplosion();

        StopTowerSpawn();

        if (gameLogic != null)
            gameLogic.gameOverScreen();

        if (pooledObject != null)
            pooledObject.ReturnToPool();
    }

    void SpawnExplosion()
    {
        if (explosionPool == null) return;

        var exp = explosionPool.Get(transform.position, Quaternion.identity);
        if (exp == null) return;

        var pooled = exp.GetComponent<PooledObject>();
        if (pooled != null) pooled.SetPool(explosionPool);
    }

    void StopTowerSpawn()
    {
        var spawner = FindAnyObjectByType<towerSpawnScript>();
        if (spawner != null)
            spawner.collisionSpawnStop = false;
    }
}
