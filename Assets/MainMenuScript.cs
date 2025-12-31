using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public AudioSource BGAudio;
    public float GameDelayDuration = 5f;

    private void Start()
    {
        if (BGAudio != null) BGAudio.Play();

        // Safety reset (prevents “stuck” when coming from paused gameplay)
        Time.timeScale = 1f;
        PauseMenuScript.GameIsPaused = false;
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

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();

    }

}
