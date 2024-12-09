using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform link;
    //public GameObject canvas;
    public Transform pauseScreen;
    public Transform gameOverScreen;
    public Transform finalScreen;

    private void Awake()
    {
        if (instance != null && instance != this){
            //Destroy(canvas);
            Destroy(gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(canvas);
        DontDestroyOnLoad(gameObject);
    }

    private bool paused;
    public void PressPause()
    {
        Debug.Log("Pause button pressed; Now paused is " + paused);
        paused = !paused;
        pauseScreen.gameObject.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
    }

    public void QuitApplication()
    {
        Debug.Log("QuitApplication");
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
        gameOverScreen.gameObject.SetActive(false);
        finalScreen.gameObject.SetActive(false);
    }
}
