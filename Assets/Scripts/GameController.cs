using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static NutrientSpawner nutrientSpawner;

    public Dictionary<string, int> scores = new Dictionary<string, int>();
    public TextMeshProUGUI scoreText;
    public GameObject winFrame;
    public TextMeshProUGUI winText;

    void Awake() {
        GameController.nutrientSpawner = GameObject.FindFirstObjectByType<NutrientSpawner>();
    }

    void Start()
    {
        scores["Player_1"] = 0;
        scores["Player_2"] = 0;
        winFrame.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetGame();
        }
        UpdateScores();
    }
    void UpdateScores()
    {
        string scoreString = "Player 1: " + scores["Player_1"] + "\n" + "Player 2: " + scores["Player_2"];
        scoreText.text = scoreString;
    }

    public void AddScore(string player, int score)
    {
        scores[player] += score;
    }

    public void TimeUp()
    {
        winFrame.SetActive(true);
        if (scores["Player_1"] > scores["Player_2"])
        {
            winText.text = "Player 1 Wins!";
        }
        if (scores["Player_1"] < scores["Player_2"])
        {
            winText.text = "Player 2 Wins!";
        }
        if (scores["Player_1"] == scores["Player_2"])
        {
            winText.text = "Tie!";
        }
        //Time.timeScale = 0;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
