using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    public Transform player;
    public float speed = 6f;
    public float predictionTime = 0.5f;
    public float lifeTime = 8f;

    private Vector2 moveDirection;

    public gameLogicScript scoreOfGame;

    [Header("Audio")]
    public AudioSource rocketSound;
    public AudioClip collisionSound;

    [Header("Effects")]
    public GameObject explosionEffect;

    public bool rocketCollisionMove = true;

    [Header("Chase Settings")]
    public float homingDuration = 1.5f;
    public float turnSpeed = 90f;

    private bool hasCollided = false;
    private float homingTimer;

    public enum RocketState
    {
        Chase,
        Wait,
        Decoy
    }

    public RocketState currentState;

    private float waitTimer = 1f;

    private void Start()
    {
        homingTimer = homingDuration;

        GameObject logicObject =
            GameObject.FindGameObjectWithTag("Logic");

        if (logicObject != null)
        {
            scoreOfGame =
                logicObject.GetComponent<gameLogicScript>();
        }

        if (player != null)
        {
            Rigidbody2D playerRb =
                player.GetComponent<Rigidbody2D>();

            Vector2 playerVelocity = Vector2.zero;

            if (playerRb != null)
            {
                playerVelocity = playerRb.linearVelocity;
            }

            Vector2 futurePos =
                (Vector2)player.position +
                playerVelocity * predictionTime;

            moveDirection =
                (futurePos -
                 (Vector2)transform.position).normalized;

            // Random rocket behaviour
            int roll = Random.Range(0, 100);

            if (roll < 60)
            {
                currentState = RocketState.Chase;
            }
            else if (roll < 80)
            {
                currentState = RocketState.Wait;
            }
            else
            {
                currentState = RocketState.Decoy;
            }
        }

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (!rocketCollisionMove)
        {
            return;
        }

        switch (currentState)
        {
            case RocketState.Chase:
                HandleChase();
                break;

            case RocketState.Wait:
                HandleWait();
                break;

            case RocketState.Decoy:
                HandleDecoy();
                break;
        }
    }

    private void HandleChase()
    {
        if (player == null)
        {
            Destroy(gameObject);
            return;
        }

        if (homingTimer > 0f)
        {
            homingTimer -= Time.deltaTime;

            Vector2 targetDirection =
                ((Vector2)player.position -
                 (Vector2)transform.position).normalized;

            float currentAngle =
                Mathf.Atan2(
                    moveDirection.y,
                    moveDirection.x
                ) * Mathf.Rad2Deg;

            float targetAngle =
                Mathf.Atan2(
                    targetDirection.y,
                    targetDirection.x
                ) * Mathf.Rad2Deg;

            float newAngle =
                Mathf.MoveTowardsAngle(
                    currentAngle,
                    targetAngle,
                    turnSpeed * Time.deltaTime
                );

            moveDirection =
                new Vector2(
                    Mathf.Cos(
                        newAngle * Mathf.Deg2Rad
                    ),
                    Mathf.Sin(
                        newAngle * Mathf.Deg2Rad
                    )
                );
        }

        transform.position +=
            (Vector3)moveDirection *
            speed *
            Time.deltaTime;

        // Destroy rocket after it passes player.
        if (transform.position.x <
            player.position.x - 1.5f)
        {
            Destroy(gameObject);
            return;
        }

        // Destroy if outside screen/world bounds.
        if (transform.position.x < -10f ||
            transform.position.x > 10f ||
            transform.position.y < -6f ||
            transform.position.y > 6f)
        {
            Destroy(gameObject);
        }
    }

    private void HandleWait()
    {
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0f)
        {
            currentState = RocketState.Chase;
        }
    }

    private void HandleDecoy()
    {
        transform.position +=
            Vector3.left *
            speed *
            Time.deltaTime;
    }

    private void OnCollisionEnter2D(
        Collision2D collision)
    {
        if (hasCollided)
        {
            return;
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        hasCollided = true;

        // Stop rocket immediately.
        rocketCollisionMove = false;
        speed = 0f;
        moveDirection = Vector2.zero;

        Collider2D rocketCollider =
            GetComponent<Collider2D>();

        if (rocketCollider != null)
        {
            rocketCollider.enabled = false;
        }


        Destroy(gameObject);

        // Explosion visual.
        if (explosionEffect != null)
        {
            Instantiate(
                explosionEffect,
                transform.position,
                Quaternion.identity
            );
        }

        // Explosion sound.
        if (collisionSound != null)
        {
            Vector3 soundPosition =
                Camera.main != null
                ? Camera.main.transform.position
                : transform.position;

            AudioSource.PlayClipAtPoint(
                collisionSound,
                soundPosition,
                0.7f
            );
        }

        // Stop spawning more rockets.
        StopRocketSpawning();

        // Game over.
        if (scoreOfGame != null)
        {
            scoreOfGame.gameOverScreen();
        }

        // Destroy player.
        Destroy(collision.gameObject);
    }

    private void StopRocketSpawning()
    {
        rocketSpawnScript rocketSpawner =
            FindFirstObjectByType<rocketSpawnScript>();

        if (rocketSpawner != null)
        {
            rocketSpawner.StopRocketSpawning();
        }
    }
}