using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    public Transform player;
    public float speed = 6f;
    public float predictionTime = 0.5f;
    public float lifeTime = 8f;
    private Vector2 moveDirection;

    public gameLogicScript scoreOfGame;
    public AudioSource rocketSound;
    public GameObject explosionEffect;

    public bool rocketCollisionMove = true;

    [Header("Chase Settings")]
    public float homingDuration = 1.5f;
    public float turnSpeed = 90f;

    private float homingTimer;
    // Rocket States
    public enum RocketState { Chase, Wait, Decoy }
    public RocketState currentState;
    private float waitTimer = 1f; // how long rockets "charge" before moving

    void Start()
    {
        homingTimer = homingDuration;
        scoreOfGame = GameObject.FindGameObjectWithTag("Logic").GetComponent<gameLogicScript>();

        if (player != null)
        {
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            Vector2 futurePos = (Vector2)player.position + playerRb.linearVelocity * predictionTime;
            moveDirection = (futurePos - (Vector2)transform.position).normalized;

            // Randomize rocket type
            int roll = Random.Range(0, 100);
            if (roll < 60) currentState = RocketState.Chase;   // 60% chance
            else if (roll < 80) currentState = RocketState.Wait;  // 20% chance
            else currentState = RocketState.Decoy;   // 20% chance
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (!rocketCollisionMove) return;

        // State-based behavior
        switch (currentState)
        {
            case RocketState.Chase: HandleChase(); break;
            case RocketState.Wait: HandleWait(); break;
            case RocketState.Decoy: HandleDecoy(); break;
        }
    }

    void HandleChase()
    {
        if (player == null)
        {
            Destroy(gameObject);
            return;
        }

        // Track the player only for a limited period.
        if (homingTimer > 0f)
        {
            homingTimer -= Time.deltaTime;

            Vector2 targetDirection =
                ((Vector2)player.position -
                 (Vector2)transform.position).normalized;

            float currentAngle =
                Mathf.Atan2(moveDirection.y, moveDirection.x) *
                Mathf.Rad2Deg;

            float targetAngle =
                Mathf.Atan2(targetDirection.y, targetDirection.x) *
                Mathf.Rad2Deg;

            float newAngle = Mathf.MoveTowardsAngle(
                currentAngle,
                targetAngle,
                turnSpeed * Time.deltaTime
            );

            moveDirection = new Vector2(
                Mathf.Cos(newAngle * Mathf.Deg2Rad),
                Mathf.Sin(newAngle * Mathf.Deg2Rad)
            );
        }

        // After the timer finishes, it continues straight.
        transform.position +=
            (Vector3)moveDirection * speed * Time.deltaTime;

        // The rocket cannot turn around after passing the player.
        if (transform.position.x < player.position.x - 1.5f)
        {
            Destroy(gameObject);
            return;
        }

        if (transform.position.x < -10f ||
            transform.position.x > 10f ||
            transform.position.y < -6f ||
            transform.position.y > 6f)
        {
            Destroy(gameObject);
        }
    }

    void HandleWait()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0) currentState = RocketState.Chase;
    }

    void HandleDecoy()
    {
        // Just fly straight left (ignores player)
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Explosion effect
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            if (rocketSound != null && rocketSound.clip != null)
            {
                AudioSource.PlayClipAtPoint(
                    rocketSound.clip,
                    transform.position,
                    rocketSound.volume
                );
            }

            scoreOfGame.gameOverScreen();

            StopRocketSpawning();

            Destroy(collision.gameObject);

            Destroy(gameObject);
        }
    }

    void StopRocketSpawning()
    {
        rocketSpawnScript rocketSpawner = FindFirstObjectByType<rocketSpawnScript>();
        if (rocketSpawner != null)
        {
            rocketSpawner.StopRocketSpawning();
        }
    }
}
