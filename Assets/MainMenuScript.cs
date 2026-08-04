using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public AudioSource BGAudio;
    public float GameDelayDuration = 5f;

    [Header("How To Play")]
    public GameObject howToPlayPanel;

    private void Start()
    {
        if (BGAudio != null) { BGAudio.Play(); }

        Time.timeScale = 1f;
        PauseMenuScript.GameIsPaused = false;

        if (howToPlayPanel != null) {  howToPlayPanel.SetActive(false); }
    }

    public void PlayGame()
    {
        StartCoroutine(LoadGameDelay());

    }

    private IEnumerator LoadGameDelay()
    {
        yield return new WaitForSeconds(GameDelayDuration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }


    public void OpenHowToPlay()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(true);
        }
    }

    public void CloseHowToPlay()
    {
        if (howToPlayPanel != null)
        {
            howToPlayPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();

    }

}
