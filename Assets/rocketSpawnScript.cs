using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketSpawnScript : MonoBehaviour
{
    [Header("References")]
    public GameObject rocketLauncherPrefab;
    public gameLogicScript gameLogic;
    public Transform player;
    public AudioSource spawnSound;

    [Header("Spawn Settings")]
    public float rocketSpawnRate = 2f;
    public float delay = 3f;

    [Range(0f, 1f)]
    public float randomFireChance = 0.25f;

    public float altitudeThreshold = 3f;

    private float timer = 0f;
    private bool canSpawnRockets = false;

    private void Start()
    {
        GameObject logicObject =
                GameObject.FindGameObjectWithTag("Logic");
        if (logicObject != null)
        {
            gameLogic =
                logicObject.GetComponent<gameLogicScript>();
        }

        canSpawnRockets = false;
        timer = 0f;

        Invoke(nameof(EnableSpawning), delay);
    }

    private void Update()
    {
        if (!canSpawnRockets)
        {
            return;
        }
        if (gameLogic != null && !gameLogic.IsPlaying)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer < rocketSpawnRate)
        {
            return;
        }
        timer -= rocketSpawnRate;

        bool playerIsHigh =
            player != null &&
            player.position.y > altitudeThreshold;

        bool passedRandomChance =
            Random.value < randomFireChance;

        if (playerIsHigh || passedRandomChance)
        {
            SpawnRocket();
        }
    }

    private void EnableSpawning()
    {
        if (gameLogic != null && !gameLogic.IsPlaying)
        {
            return;
        }
        canSpawnRockets = true;
        timer = 0f;
    }
    private void SpawnRocket()
    {
        GameObject spawnedRocket = Instantiate(rocketLauncherPrefab, transform.position, Quaternion.identity);
        RocketMovement rocketMovement =
            spawnedRocket.GetComponent<RocketMovement>();

        if (rocketMovement == null)
        {
            rocketMovement =
                spawnedRocket.GetComponentInChildren<RocketMovement>();
        }
        if (rocketMovement != null)
        {
            rocketMovement.player = player;
            rocketMovement.scoreOfGame = gameLogic;
        }
        else
        {
            Debug.LogWarning(
                "The spawned rocket does not contain RocketMovement.",
                spawnedRocket
            );
        }

        if (spawnSound != null)
        {
            spawnSound.Play();
        }
    }

    public void StopRocketSpawning()
    {
        canSpawnRockets = false;
        timer = 0f;

        CancelInvoke(nameof(EnableSpawning));
    }
}
