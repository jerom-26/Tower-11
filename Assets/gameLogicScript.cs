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
    public ScrollerScript scroller;

    [Header("Difficulty")]
    public int maxDifficultyScore = 50;

    public float GetDifficulty01()
    {
        if (maxDifficultyScore <= 0) return 0f;
        return Mathf.Clamp01((float)playerScore / maxDifficultyScore);
    }

    void Start()
    {
       highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

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

    public GameObject leaderboardPanel;

    public void gameOverScreen()
    {
        if (isGameOver) return;
        isGameOver = true;

        gameOver.SetActive(true);

        towerSpawnScript towerSpawner = FindFirstObjectByType<towerSpawnScript>();
        if (towerSpawner != null)
            towerSpawner.StopCollisionSpawning();

        rocketSpawnScript rocketSpawner = FindFirstObjectByType<rocketSpawnScript>();
        if (rocketSpawner != null)
            rocketSpawner.StopRocketSpawning();

        if (scroller != null)
            scroller.enabled = false;

        leaderboardManager.SubmitScore(playerScore);
        ShowLeaderboard();
    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        leaderboardManager.LoadLeaderboard();

    }
}

