using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Dictionary<string, int> scores = new Dictionary<string, int>();
    public TextMeshProUGUI scoreText;
    void Start()
    {
        scores["Player_1"] = 0;
        scores["Player_2"] = 0;
    }

    void Update()
    {
        UpdateScores();
    }
    void UpdateScores()
    {
        string scoreString = "Player 1: " + scores["Player_1"] + "\n" + "Player 2: " + scores["Player_2"];
        scoreText.text = scoreString;
    }

    public void SetScore(string player, int score)
    {
        scores[player] = score;
    }
}
