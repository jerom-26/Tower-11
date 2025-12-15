using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public AudioSource BGAudio;
    public float GameDelayDuration = 2f;
    public void PlayGame()
    {
        StartCoroutine(LoadGameDelay());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    private IEnumerator LoadGameDelay()
    {
        yield return new WaitForSeconds(GameDelayDuration);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();

    }

    private void Start()
    {
        BGAudio.Play();
    }
}
