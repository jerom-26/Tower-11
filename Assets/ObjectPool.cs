using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Pool Setup")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 10;
    [SerializeField] private bool canExpand = true;

    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        // Pre-warm the pool: create objects once at start
        for (int i = 0; i < initialSize; i++)
            CreateAndStore();
        
    }

    private GameObject CreateAndStore()
    {
        var obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    // Get an object from the pool (activate it and return).
    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            if (!canExpand)
                return null;

            obj = CreateAndStore();
            pool.Dequeue();
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        return obj;
    }

    // Return an object to the pool (deactivate + store).
    public void Return (GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        pool.Enqueue(obj);
    }
}
