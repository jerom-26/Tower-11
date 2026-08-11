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

    [Header("References")]
    [SerializeField] private gameLogicScript gameLogic;

    private SpriteRenderer spriteRenderer;

    private PooledObject pooledObject;
    private Rigidbody2D rb;
    private Vector3 originalScale;

    private void Awake()
    {
        pooledObject = GetComponent<PooledObject>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;

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

    }

    public void OnSpawned()
    {
        // Reset physics state 
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        float heightMultiplier = Random.Range(minHeight, maxHeight);


        transform.localScale = new Vector3(
            originalScale.x,
            originalScale.y * heightMultiplier,
            originalScale.z
        );
        
    }

    private void Update()
    {
        // Stops existing towers before gameplay, while paused,
        // and after game over.
        if (gameLogic == null || !gameLogic.IsPlaying)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            return;
        }

        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Off-screen check and return to pool (NOT Destroy)
        if (transform.position.x < -15f) 
        {
            if (pooledObject != null)
            {
                pooledObject.ReturnToPool();
            }
        }
    }

}
