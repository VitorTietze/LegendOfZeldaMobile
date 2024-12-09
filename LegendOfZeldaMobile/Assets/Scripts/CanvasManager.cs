using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.pauseScreen = transform.Find("PauseScreen");
        GameManager.instance.gameOverScreen = transform.Find("GameOver");
        GameManager.instance.finalScreen = transform.Find("FinalScreen");

        transform.Find("PauseButton").GetComponent<Button>().onClick.AddListener(
            () => GameManager.instance.PressPause()
        );
        GameManager.instance.pauseScreen.Find("QuitButton").GetComponent<Button>().onClick.AddListener(
            () => GameManager.instance.QuitApplication()
        );
        GameManager.instance.gameOverScreen.Find("RestartButton").GetComponent<Button>().onClick.AddListener(
            () => GameManager.instance.Restart()
        );
        GameManager.instance.finalScreen.Find("RestartButton").GetComponent<Button>().onClick.AddListener(
            () => GameManager.instance.Restart()
        );
    }
}
