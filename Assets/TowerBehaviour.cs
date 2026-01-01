using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Random Height")]
    [SerializeField] private float minHeight = 0.8f;
    [SerializeField] private float maxHeight = 1.6f;

    private PooledObject pooledObject;
    private Rigidbody2D rb;

    private void Awake()
    {
        // Cache references ONCE (important for performance)
        pooledObject = GetComponent<PooledObject>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Called EVERY time the tower is reused from the pool.
    // This replaces "Start()" logic for pooled objects.
    public void OnSpawned()
    {
        // Reset physics state 
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // HARD RESET so pooling never keeps an old stretched value
        transform.localScale = Vector3.one;

        // then random height
        float h = Random.Range(minHeight, maxHeight);
        transform.localScale = new Vector3(1f, h, 1f);

    }

    private void Update()
    {
        // Move tower left every frame
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Off-screen check → return to pool (NOT Destroy)
        if (transform.position.x < -15f) // adjust for your camera
        {
            pooledObject.ReturnToPool();
        }
    }

}
