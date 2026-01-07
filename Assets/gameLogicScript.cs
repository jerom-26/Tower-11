using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameLogicScript : MonoBehaviour
{
    public int playerScore;
    public Text scoreUiText;
    public GameObject gameOver;
    public GameObject explosionEffect;
    public GameObject rocketLauncherPrefab;
    public bool rocketLauncher = true;
    public Transform rocketSpawnPosition;
    public Text highScoreText;
    public int highScore;
    public AudioSource scoreSound;
    private bool isGameOver;

    public LeaderboardManager leaderboardManager;

    [Header("Difficulty")]
    public int maxDifficultyScore = 50;

    public float GetDifficulty01()
    {
        if (maxDifficultyScore <= 0) return 0f;
        return Mathf.Clamp01((float)playerScore / maxDifficultyScore);
    }

    // Start is called before the first frame update
    void Start()
    {
       highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Add Score")]
    public void gameScore()
    {
        playerScore = playerScore + 1;
        scoreUiText.text = playerScore.ToString();
        scoreSound.Play();

        if (playerScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            highScore = playerScore;
            PlayerPrefs.SetInt("HighScore", playerScore);
            highScoreText.text = playerScore.ToString();
        }
    }
 
    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }

    public GameObject leaderboardPanel; // drag your LeaderboardPanel here in Inspector

    public void gameOverScreen()
    {
        if (isGameOver) return;
        isGameOver = true;

        // show Game Over UI
        gameOver.SetActive(true);

        // stop spawners
        towerSpawnScript towerSpawner = FindFirstObjectByType<towerSpawnScript>();
        if (towerSpawner != null)
            towerSpawner.StopCollisionSpawning();

        rocketSpawnScript rocketSpawner = FindFirstObjectByType<rocketSpawnScript>();
        if (rocketSpawner != null)
            rocketSpawner.StopRocketSpawning();

        // ✅ submit score
        leaderboardManager.SubmitScore(playerScore);
        ShowLeaderboard();
    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        leaderboardManager.LoadLeaderboard();

    }
}

