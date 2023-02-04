using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerController : MonoBehaviour
{
    public float roundTime = 0;
    private float timeRemaining;
    private bool timeIsFlowing = false;
    public TextMeshProUGUI timeText;
    public string timerPrefix;
    void Start()
    {
        timeRemaining = roundTime;
        timeIsFlowing = true;
    }

    void Update()
    {
        if (timeIsFlowing)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining < 0) { timeRemaining = 0; }
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Game End");
                timeRemaining = 0;
                timeIsFlowing = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minues = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = timerPrefix + string.Format("{0:00}:{1:00}", minues, seconds);
    }
}
