using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float predictionTime = 0.5f;
    public float lifeTime = 3f;
    private Vector2 moveDirection;

    public gameLogicScript scoreOfGame;
    public AudioSource rocketSound;
    public GameObject explosionEffect;

    public bool rocketCollisionMove = true;

    // Rocket States
    public enum RocketState { Chase, Wait, Decoy }
    public RocketState currentState;
    private float waitTimer = 1f; // how long rockets "charge" before moving

    void Start()
    {
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

        Destroy(gameObject, lifeTime); // auto destroy if no hit
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
        if (player == null) return;

        // Calculate desired direction toward player
        Vector2 targetDir = ((Vector2)player.position - (Vector2)transform.position).normalized;

        // Smoothly rotate current moveDirection toward targetDir
        moveDirection = Vector2.Lerp(moveDirection, targetDir, 0.05f).normalized;

        // Keep moving forward with momentum
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;

        // Destroy if rocket leaves play area (slightly outside camera)
        if (transform.position.x < -10f || transform.position.x > 10f ||
            transform.position.y < -6f || transform.position.y > 6f)
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

            // Play sound (detach so it isn�t cut off when rocket is destroyed)
            if (rocketSound != null)
            {
                rocketSound.transform.parent = null;
                rocketSound.Play();
                Destroy(rocketSound.gameObject, rocketSound.clip.length);
            }

            // Trigger game over
            scoreOfGame.gameOverScreen();

            // Stop spawner
            StopRocketSpawning();

            // Destroy player plane
            Destroy(collision.gameObject);

            // Destroy rocket
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
