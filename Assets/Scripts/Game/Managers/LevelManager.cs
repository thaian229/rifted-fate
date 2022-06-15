using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Text scoreText;
    public GameObject gameOverPanel;
    public int score = 0;
    public static LevelManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }

        gameOverPanel.SetActive(false);
        score = 0;
        scoreText.text = "Score: " + score;
    }

    public void EarnScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        // Handle 1 player die
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void GameOverContinue()
    {
        Time.timeScale = 1;
        SceneController.instance.SwitchScenes("MainMenu");
    }
}
