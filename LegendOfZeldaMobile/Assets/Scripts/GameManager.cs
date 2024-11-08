using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverScreen.gameObject.SetActive(true);
    }

    public void FinalScreen()
    {
        Time.timeScale = 0;
        finalScreen.gameObject.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
