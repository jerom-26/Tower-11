using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class gameLogicScript : MonoBehaviour
{
    public enum GameState
    {
        Waiting,
        Playing,
        Paused,
        GameOver
    }
    [Header("Game State")]
    public GameState currentState = GameState.Waiting;

    [Header("Gameplay References")]
    public AirplaneScript airplane;
    public towerSpawnScript towerSpawner;
    public rocketSpawnScript rocketSpawner;

    [Tooltip("Assign the username panel")]
    public GameObject usernamePanel;

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

    [Header("Background Music")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip gameplayBGM;
    [SerializeField, Range(0f, 1f)] private float bgmVolume = 0.35f;


    public bool rocketLauncher = true;
    public int highScore;

    private bool isGameOver;

    public LeaderboardManager leaderboardManager;

    [Header("Difficulty")]
    public int maxDifficultyScore = 50;
    public bool IsPlaying => currentState == GameState.Playing;
   
    private void Awake()
    {
        if (airplane == null)
        {
            airplane = FindFirstObjectByType<AirplaneScript>();
        }

        if (towerSpawner == null)
        {
            towerSpawner = FindFirstObjectByType<towerSpawnScript>();
        }

        if (rocketSpawner == null)
        {
            rocketSpawner = FindFirstObjectByType<rocketSpawnScript>();
        }

        currentState = GameState.Waiting;
        Time.timeScale = 1f;

        if (towerSpawner != null)
        {
            towerSpawner.enabled = false;
        }

        if (rocketSpawner != null)
        {
            rocketSpawner.enabled = false;
        }
    }

    void Start()
    {
        if (bgmSource != null && gameplayBGM != null)
        {
            bgmSource.clip = gameplayBGM;
            bgmSource.loop = true;
            bgmSource.volume = bgmVolume;

            if (!bgmSource.isPlaying)
            {
                bgmSource.Play();
            }
        }

        playerScore = 0;

        if (scoreUiText != null)
        {
            scoreUiText.text = "0";
            scoreUiText.gameObject.SetActive(true);
        }

        if (highScoreText != null)
        {
            highScoreText.text =
                PlayerPrefs.GetInt("HighScore", 0).ToString();
        }

        if (gameOver != null)
        {
            gameOver.SetActive(false);
        }

        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(false);
        }

        if (airplane != null)
        {
            airplane.DisableGameplay();
        }
    }

    void Update()
    {
        if (currentState != GameState.Waiting)
        {
            return;
        }
        if (usernamePanel != null && usernamePanel.activeInHierarchy)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            BeginGame();
        }

    }

    public void BeginGame()
    {
        if (currentState != GameState.Waiting)
        {
            return;
        }

        currentState = GameState.Playing;

        if (airplane != null)
        {
            airplane.BeginGame();
        }

        if (towerSpawner != null)
        {
            towerSpawner.enabled = true;
        }

        if (rocketSpawner != null)
        {
            rocketSpawner.enabled = true;
        }
    }
    public float GetDifficulty01()
    {
        if (maxDifficultyScore <= 0)
        {
            return 0f;
        }

        return Mathf.Clamp01(
            (float)playerScore / maxDifficultyScore
        );
    }

    [ContextMenu("Add Score")]
    public void gameScore()
    {
        if (currentState != GameState.Playing) 
        {
            return;
        }

        playerScore++;

        if (scoreUiText != null)
        {
            scoreUiText.text = playerScore.ToString();
        }
        scoreSound.Play();

        int savedHighScore =
            PlayerPrefs.GetInt("HighScore", 0);

        if (playerScore > savedHighScore)
        {
            highScore = playerScore;
            PlayerPrefs.SetInt("HighScore", playerScore);
            PlayerPrefs.Save();
            highScoreText.text = playerScore.ToString();
        }
    }
 
    public void playAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOverScreen()
    {
        if (currentState == GameState.GameOver)
        {
            return;
        }

        currentState = GameState.GameOver;

        if (airplane != null)
        {
            airplane.DisableGameplay();
        }

        if (towerSpawner != null)
        {
            towerSpawner.StopCollisionSpawning();
        }

        if (rocketSpawner != null)
        {
            rocketSpawner.StopRocketSpawning();
        }

        if (scoreUiText != null)
        {
            scoreUiText.gameObject.SetActive(false);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text =
                $"SCORE <color=#FF79C9>{playerScore}</color>";
        }

        if (gameOver != null)
        {
            gameOver.SetActive(true);
        }

        if (leaderboardManager != null)
        {
            leaderboardPanel.SetActive(true);
            leaderboardManager.SubmitScore(playerScore);
        }
    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        leaderboardManager.LoadLeaderboard();

    }
}

