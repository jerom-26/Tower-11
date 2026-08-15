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
    [SerializeField] private Transform airplane;

    private SpriteRenderer spriteRenderer;
    private PooledObject pooledObject;
    private Rigidbody2D rb;

    private Vector3 originalScale;


    private bool scoreAwarded = false;

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

        if (airplane == null)
        {
            AirplaneScript plane =
                FindFirstObjectByType<AirplaneScript>();

            if (plane != null)
            {
                airplane = plane.transform;
            }
        }
    }

    public void OnSpawned()
    {

        scoreAwarded = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        float heightMultiplier =
            Random.Range(minHeight, maxHeight);

        transform.localScale =
            new Vector3(
                originalScale.x,
                originalScale.y * heightMultiplier,
                originalScale.z
            );
    }

    private void Update()
    {
        if (gameLogic == null || !gameLogic.IsPlaying)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            return;
        }

        float previousX = transform.position.x;

        // Move tower.
        transform.position +=
            Vector3.left * moveSpeed * Time.deltaTime;

        float currentX = transform.position.x;


        if (!scoreAwarded && airplane != null)
        {
            float planeX = airplane.position.x;

            if (previousX >= planeX &&
                currentX < planeX)
            {
                scoreAwarded = true;

                gameLogic.gameScore();
            }
        }

   
        if (transform.position.x < -15f)
        {
            if (pooledObject != null)
            {
                pooledObject.ReturnToPool();
            }
        }
    }
}