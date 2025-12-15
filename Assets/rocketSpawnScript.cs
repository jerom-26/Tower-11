using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketSpawnScript : MonoBehaviour
{
    public GameObject rocketLauncherPrefab;
    public gameLogicScript gameLogic;
    public float rocketSpawnRate = 2f;
    private float timer = 0f;
    private bool canSpawnRockets = true;

    public Transform player; // assign airplane in inspector
    public AudioSource spawnSound;

    void Start()
    {
        gameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<gameLogicScript>();
        SpawnRocket(); // spawn one at start
    }

    void Update()
    {
        if (!canSpawnRockets)
            return;

        timer += Time.deltaTime;

        if (timer >= rocketSpawnRate)
        {
            bool shouldFire = false;

            // Condition 1: Player altitude
            if (player != null && player.position.y > 3f)
                shouldFire = true;

            //Condition 2: Random chance
            if (Random.value < 0.25f)
                shouldFire = true;

            if (shouldFire)
            {
                SpawnRocket();
            }

            timer = 0;
        }
    }

    void SpawnRocket()
    {
        Instantiate(rocketLauncherPrefab, transform.position, Quaternion.identity);

        if (spawnSound != null)
        {
            spawnSound.Play();
        }
    }

    public void StopRocketSpawning()
    {
        canSpawnRockets = false;
    }
}
