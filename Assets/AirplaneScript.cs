using UnityEngine;

public class AirplaneScript : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody2D myRigidBody;
    public float planeVelocity = 7.5f;
    public float activeGravityScale = 1.8f;
    public float noFlyZoneUpper = 8f;
    public float noFlyZoneLower = -4f;

    [Header("Bounds")]
    public float boundsPadding = 0.15f;

    [Header("Tilt Settings")]
    public float tiltMultiplier = 6f;
    public float maxUpAngle = 30f;
    public float maxDownAngle = -60f;
    public float tiltSmooth = 8f;

    [Header("Optional Visual")]
    public Transform planeVisual;

    [Header("Game Logic")]
    public gameLogicScript gameLogic;

    [Header("FX")]
    public GameObject explosionEffect;
    public AudioSource explosionSound;

    private bool planeAlive = true;
    private bool gameplayEnabled = false;
    private bool flapRequested = false;

    [SerializeField] private AudioSource flapAudioSource;
    [SerializeField] private AudioClip flapSound;
    [SerializeField, Range(0f, 1f)] private float flapVolume = 0.2f;

    private void Awake()
    {
        if (myRigidBody == null)
        {
            myRigidBody = GetComponent<Rigidbody2D>();
        }

        if (planeVisual == null)
        {
            planeVisual = transform;
        }

        if (myRigidBody != null)
        {
            myRigidBody.gravityScale = 0f;
            myRigidBody.linearVelocity = Vector2.zero;
            myRigidBody.angularVelocity = 0f;

            myRigidBody.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    private void Start()
    {
        if (gameLogic == null)
        {
            GameObject logicObject = GameObject.FindGameObjectWithTag("Logic");

            if (logicObject != null)
            {
                gameLogic = logicObject.GetComponent<gameLogicScript>();
            }
        }
    }

    private void Update()
    {
        if (!planeAlive || !gameplayEnabled)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            flapRequested = true;
        }

        ApplyTilt();
    }

    private void FixedUpdate()
    {
        if (!planeAlive || !gameplayEnabled || myRigidBody == null)
        {
            return;
        }

        if (flapRequested)
        {
            Flap();
            flapRequested = false;
        }

        ClampToBounds();
    }

    public void BeginGame()
    {
        if (!planeAlive || gameplayEnabled || myRigidBody == null)
        {
            return;
        }

        gameplayEnabled = true;
        flapRequested = false;

        myRigidBody.linearVelocity = Vector2.zero;
        myRigidBody.angularVelocity = 0f;
        myRigidBody.gravityScale = activeGravityScale;

        Flap();
    }

    public void DisableGameplay()
    {
        gameplayEnabled = false;
        flapRequested = false;

        if (myRigidBody == null)
        {
            return;
        }

        myRigidBody.gravityScale = 0f;
        myRigidBody.linearVelocity = Vector2.zero;
        myRigidBody.angularVelocity = 0f;
    }

    private void Flap()
    {
        Vector2 velocity = myRigidBody.linearVelocity;
        velocity.y = planeVelocity;
        myRigidBody.linearVelocity = velocity;

        if (flapAudioSource != null && flapSound != null)
        {
            flapAudioSource.PlayOneShot(flapSound, flapVolume);
        }
    }

    private void ClampToBounds()
    {
        Vector2 pos = myRigidBody.position;

        float maxY = noFlyZoneUpper - boundsPadding;

        if (pos.y > maxY)
        {
            pos.y = maxY;

            myRigidBody.position = pos;

            if (myRigidBody.linearVelocity.y > 0f)
            {
                Vector2 velocity = myRigidBody.linearVelocity;
                velocity.y = 0f;
                myRigidBody.linearVelocity = velocity;
            }
        }

        if (pos.y < noFlyZoneLower)
        {
            Die();
        }
    }

    private void ApplyTilt()
    {
        if (myRigidBody == null || planeVisual == null)
        {
            return;
        }

        float targetAngle =
            myRigidBody.linearVelocity.y * tiltMultiplier;

        targetAngle =
            Mathf.Clamp(targetAngle, maxDownAngle, maxUpAngle);

        Quaternion desiredRotation =
            Quaternion.Euler(0f, 0f, targetAngle);

        planeVisual.rotation = Quaternion.Slerp(
            planeVisual.rotation,
            desiredRotation,
            tiltSmooth * Time.deltaTime
        );
    }

    public void KillPlane()
    {
        Die();
    }

    private void Die()
    {
        if (!planeAlive)
        {
            return;
        }

        planeAlive = false;
        gameplayEnabled = false;
        flapRequested = false;

        if (myRigidBody != null)
        {
            myRigidBody.linearVelocity = Vector2.zero;
            myRigidBody.angularVelocity = 0f;
            myRigidBody.gravityScale = 0f;
        }

        if (explosionEffect != null)
        {
            Instantiate(
                explosionEffect,
                transform.position,
                Quaternion.identity
            );
        }

        if (explosionSound != null)
        {
            explosionSound.Play();
        }

        if (gameLogic != null)
        {
            gameLogic.gameOverScreen();
        }

        SpriteRenderer spriteRenderer =
            GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        Collider2D planeCollider =
            GetComponent<Collider2D>();

        if (planeCollider != null)
        {
            planeCollider.enabled = false;
        }

        if (explosionSound != null)
        {
            Destroy(gameObject, 2f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!planeAlive || !gameplayEnabled)
        {
            return;
        }

        if (
            collision.gameObject.CompareTag("Tower") ||
            collision.gameObject.CompareTag("Rocket")
        )
        {
            Die();
        }
    }
}