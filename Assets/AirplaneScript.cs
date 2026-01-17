using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneScript : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody2D myRigidBody;
    public float planeVelocity = 7.5f;
    public float noFlyZoneUpper = 8f;
    public float noFlyZoneLower = -4f;

    [Header("Bounds")]
    public float boundsPadding = 0.15f;

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

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Flap();
        }

        // Apply tilt based on current vertical speed
        ApplyTilt();
    }

    void LateUpdate()
    {
        if (!planeAlive) return;
        if (!gameStarted) return;

        ClampToBounds();
    }

    void ClampToBounds()
    {
        Vector3 pos = transform.position;

        float maxY = noFlyZoneUpper - boundsPadding;

        // Clamp only the TOP
        if (pos.y > maxY)
        {
            pos.y = maxY;
            transform.position = pos;
        }

        // BOTTOM = death
        if (pos.y < noFlyZoneLower)
        {
            Die();
        }
    }


    void Flap()
    {
        // reset vertical speed then push up -> snappy control
        myRigidBody.linearVelocity = new Vector2(myRigidBody.linearVelocity.x, 0f);
        myRigidBody.linearVelocity += Vector2.up * planeVelocity;
    }

    void ApplyTilt()
    {
        // Convert vertical velocity into an angle
        float targetAngle = myRigidBody.linearVelocity.y * tiltMultiplier;

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
