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
