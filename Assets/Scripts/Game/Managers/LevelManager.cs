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
    public const int REWARD_PER_SCORE = 50;
    public const int WIN_REWARD = 200;

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

        EventManager.AddListener<AllObjectivesCompletedEvent>(OnFinishObjectives);

        gameOverPanel.SetActive(false);
        score = 0;
        scoreText.color = Color.yellow;
        scoreText.text = "Score: " + score;
    }

    public void EarnScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }

    public void GameOver(bool isWon)
    {
        // Handle 1 player die
        Time.timeScale = 0;
        Text overText = gameOverPanel.transform.Find("OverText").GetComponent<Text>();
        Text rewardText = gameOverPanel.transform.Find("RewardText").GetComponent<Text>();

        
        int reward = this.score * REWARD_PER_SCORE;
        if (isWon) {
            reward += WIN_REWARD;
            overText.text = "YOU WON";
            overText.color = Color.green;
        } else {
            overText.text = "YOU DIED";
            overText.color = Color.red;
        }
        rewardText.text = "+" + reward + " C";
        gameOverPanel.SetActive(true);
        GameManager.instance.AddCredit(reward);
    }

    public void GameOverContinue()
    {
        Time.timeScale = 1;
        SceneController.instance.SwitchScenes("MainMenu");
    }

    void OnFinishObjectives(AllObjectivesCompletedEvent evt)
    {
        GameOver(true);
    }

    void OnDestroy()
    {
        EventManager.RemoveListener<AllObjectivesCompletedEvent>(OnFinishObjectives);
    }
}
