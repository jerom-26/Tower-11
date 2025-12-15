using UnityEngine;
using TMPro;

public class UsernameManager : MonoBehaviour
{
    [Header("References")]
    public TMP_InputField usernameInput;
    public GameObject usernamePanel;      // assign this specific panel
    public GameObject gameOverPanel;      // optional
    public GameObject leaderboardPanel;   // optional

    void Awake()
    {
        if (usernamePanel == null)
            Debug.LogError("UsernamePanel is not assigned in Inspector.");
        if (usernameInput == null)
            Debug.LogError("UsernameInput is not assigned in Inspector.");
    }


    void Start()
    {
        if (usernamePanel == null || usernameInput == null)
        {
            Debug.LogError("UsernameManager disabled: missing references.");
            enabled = false;
            return;
        }

        if (leaderboardPanel != null)
            leaderboardPanel.SetActive(false);

        // Game Over should be hidden at the beginning too
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);


        // If username is already saved
        if (PlayerPrefs.HasKey("username"))
        {
            usernamePanel.SetActive(false);
            Time.timeScale = 1f;
            Debug.Log("Welcome back, " + PlayerPrefs.GetString("username"));
        }
        else
        {
            // Pause gameplay, show username input
            Time.timeScale = 0f;
            usernamePanel.SetActive(true);

            // Optional: hide other UI so only username panel is visible
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (leaderboardPanel != null) leaderboardPanel.SetActive(false);

            Debug.Log("Waiting for username input...");
        }
    }

    public void SaveUsername()
    {
        string username = usernameInput.text.Trim();
        string lowercaseUsername = username.ToLowerInvariant();

        if (string.IsNullOrEmpty(lowercaseUsername))
        {
            Debug.LogWarning("⚠️ Username cannot be empty!");
            return;
        }
        
        if (lowercaseUsername.Length < 3 || lowercaseUsername.Length > 20)
        {
            Debug.Log("Invalid username: must be between 3 to 20 characters");
            return;
        }

        for (int i = 0; i < lowercaseUsername.Length; i++)
        {
            char c = lowercaseUsername[i];

            bool isLowerLetter = (c >= 'a' && c <= 'z');
            bool isDigit = (c >= '0' && c <= '9');
            bool isUnderscore = (c == '_');

            if (!(isLowerLetter || isDigit || isUnderscore))
            {
                Debug.Log("Invalid username: only a-z, 0-9, and _ allowed.");
                return;
            }
        }

        // Save locally
        PlayerPrefs.SetString("username", lowercaseUsername);
        PlayerPrefs.Save();
       
        Debug.Log("✅ Username saved: " + lowercaseUsername);

        // Hide input UI and resume game
        usernamePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}