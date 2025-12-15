using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneScript : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody2D myRigidBody;
    public float planeVelocity = 7.5f;
    public float noFlyZoneUpper = 8f;
    public float noFlyZoneLower = -5.7f;

    [Header("Tilt Settings")]
    public float tiltMultiplier = 6f;     // how fast angle reacts to velocity
    public float maxUpAngle = 30f;        // degrees
    public float maxDownAngle = -60f;     // degrees
    public float tiltSmooth = 8f;         // how smoothly to rotate

    [Header("Game Logic")]
    public gameLogicScript gameLogic;

    [Header("FX")]
    public GameObject explosionEffect;
    public AudioSource explosionSound;

    bool planeAlive = true;
    bool gameStarted = false;

    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<gameLogicScript>();

        if (myRigidBody == null)
            myRigidBody = GetComponent<Rigidbody2D>();

        // Lighter feel
        myRigidBody.gravityScale = 1.8f;
    }

    void Update()
    {
        if (!planeAlive) return;

        // -------- Before Game Starts ----------
        if (!gameStarted)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                gameStarted = true;
                myRigidBody.gravityScale = 1.8f;
                Flap();        // initial jump
            }
            return;
        }

        // -------- After Game Starts ----------
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Flap();
        }

        // Kill if out of bounds
        if (transform.position.y > noFlyZoneUpper || transform.position.y < noFlyZoneLower)
        {
            Die();
        }

        // Apply tilt based on current vertical speed
        ApplyTilt();
    }


    void Flap()
    {
        // reset vertical speed then push up -> snappy control
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0f);
        myRigidBody.velocity += Vector2.up * planeVelocity;
    }

    void ApplyTilt()
    {
        // Convert vertical velocity into an angle
        float targetAngle = myRigidBody.velocity.y * tiltMultiplier;

        // Clamp so it never goes crazy
        targetAngle = Mathf.Clamp(targetAngle, maxDownAngle, maxUpAngle);

        // Smoothly rotate towards that angle
        Quaternion desiredRotation = Quaternion.Euler(0f, 0f, targetAngle);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            desiredRotation,
            tiltSmooth * Time.deltaTime
        );
    }

    void Die()
    {
        planeAlive = false;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        if (explosionSound != null)
        {
            explosionSound.Play();
        }

        if (gameLogic != null)
        {
            gameLogic.gameOverScreen();
        }

        GetComponent<SpriteRenderer>().enabled = false;

        if (explosionSound != null && explosionSound.clip != null)
            Destroy(gameObject, explosionSound.clip.length);
        else
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!planeAlive) return;

        if (collision.gameObject.CompareTag("Tower") ||
            collision.gameObject.CompareTag("Rocket"))
        {
            Die();
        }
    }
}
