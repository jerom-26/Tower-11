using UnityEngine;

public class towerExplosionHandler : MonoBehaviour
{
    private ObjectPool explosionPool;
    private PooledObject pooledObject;

    private void Awake()
    {
        pooledObject = GetComponent<PooledObject>();
    }

    private void Start()
    {
        GameObject explosionPoolObject =
            GameObject.Find("ExplosionPool");

        if (explosionPoolObject != null)
        {
            explosionPool =
                explosionPoolObject.GetComponent<ObjectPool>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AirplaneScript airplane =
            collision.gameObject.GetComponentInParent<AirplaneScript>();

        if (airplane == null)
        {
            return;
        }

        // Tower explosion.
        SpawnExplosion();

        airplane.KillPlane();

        // Return the tower to its pool.
        if (pooledObject != null)
        {
            pooledObject.ReturnToPool();
        }
    }

    private void SpawnExplosion()
    {
        if (explosionPool == null)
        {
            return;
        }

        GameObject explosion = explosionPool.Get(
            transform.position,
            Quaternion.identity
        );

        if (explosion == null)
        {
            return;
        }

        PooledObject pooledExplosion =
            explosion.GetComponent<PooledObject>();

        if (pooledExplosion != null)
        {
            pooledExplosion.SetPool(explosionPool);
        }
    }
}