using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class gameLogicScript : MonoBehaviour
{
    public int playerScore;

    [Header("Score UI")]
    public Text scoreUiText;
    public Text highScoreText;
    public Text finalScoreText;

    [Header("Game Over")]
    public GameObject gameOver;
    public GameObject leaderboardPanel;

    [Header("Effects")]
    public GameObject explosionEffect;
    public GameObject rocketLauncherPrefab;
    public Transform rocketSpawnPosition;
    public AudioSource scoreSound;


    public bool rocketLauncher = true;
    public int highScore;

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

    public void gameOverScreen()
    {

        if (isGameOver) return;
        isGameOver = true;

        gameOver.SetActive(true);

        scoreUiText.gameObject.SetActive(false);
        gameOver.SetActive(true);

        finalScoreText.text =
        $"SCORE <color=#FF79C9>{playerScore}</color>";

        towerSpawnScript towerSpawner = FindFirstObjectByType<towerSpawnScript>();
        if (towerSpawner != null)
            towerSpawner.StopCollisionSpawning();

        rocketSpawnScript rocketSpawner = FindFirstObjectByType<rocketSpawnScript>();
        if (rocketSpawner != null)
            rocketSpawner.StopRocketSpawning();

        if (scroller != null)
            scroller.enabled = false;

        if (leaderboardManager != null)
        {
            leaderboardManager.SubmitScore(playerScore);
            ShowLeaderboard();
        }

    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        leaderboardManager.LoadLeaderboard();

    }
}

