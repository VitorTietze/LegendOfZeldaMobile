using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform link;
    public Transform pauseScreen;
    public Transform gameOverScreen;
    public Transform finalScreen;

    private void Awake()
    {
        if (instance != null && instance != this){
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private bool paused;
    public void PressPause()
    {
        paused = !paused;
        pauseScreen.gameObject.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void FinalScreen()
    {
        finalScreen.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
