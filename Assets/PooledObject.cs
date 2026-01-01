using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private ObjectPool pool;

    public void SetPool(ObjectPool owningPool)
    {
        pool = owningPool;
    }

    public void ReturnToPool()
    {
        if (pool == null)
        {
            gameObject.SetActive(false);
            return;
        }

        pool.Return(gameObject);
    }
}
