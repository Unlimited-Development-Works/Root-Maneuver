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
    private bool showWinScreen = false;

    void Awake() {
        GameController.nutrientSpawner = GameObject.FindFirstObjectByType<NutrientSpawner>();
    }

    void Start()
    {
        scores["Player_1"] = 0;
        scores["Player_2"] = 0;
        winFrame.SetActive(false);
        PutColliders();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetGame();
        }
        if (showWinScreen)
        {
            if (Input.GetButtonDown("Reset"))
            {
                ResetGame();
            }
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
        showWinScreen = true;
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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("TitleScene");
    }

    private void PutColliders()
    {
        GameObject left = new GameObject("LeftCollider", typeof(BoxCollider2D), typeof(Rigidbody2D));
        left.layer = 6;
        left.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GameObject right = new GameObject("RightCollider", typeof(BoxCollider2D));
        right.layer = 6;
        right.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //GameObject top = new GameObject("TopCollider", typeof(BoxCollider2D));
        GameObject bottom = new GameObject("BottomCollider", typeof(BoxCollider2D));
        bottom.layer = 6;
        bottom.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        left.transform.SetParent(transform);
        right.transform.SetParent(transform);
        //top.transform.SetParent(transform);
        bottom.transform.SetParent(transform);

        Vector2 leftBottomCorner = mainCamera.ViewportToWorldPoint(Vector3.zero);
        Vector2 rightTopCorner = mainCamera.ViewportToWorldPoint(Vector3.one);

        Debug.LogWarning(leftBottomCorner);
        Debug.Log(rightTopCorner);

        left.transform.position = new Vector2(
            leftBottomCorner.x - 0.5f,
            mainCamera.transform.position.y
            );
        right.transform.position = new Vector2(
            rightTopCorner.x + 0.5f,
            mainCamera.transform.position.y
            );
/*        top.transform.position = new Vector2(
            mainCamera.transform.position.x,
            rightTopCorner.y + 0.5f
            );*/
        bottom.transform.position = new Vector2(
            mainCamera.transform.position.x,
            leftBottomCorner.y - 0.5f
            );

        left.transform.localScale = new Vector3(1, Mathf.Abs(rightTopCorner.y - leftBottomCorner.y));
        right.transform.localScale = new Vector3(1, Mathf.Abs(rightTopCorner.y - leftBottomCorner.y));
        //top.transform.localScale = new Vector3(Mathf.Abs(rightTopCorner.x - leftBottomCorner.x), 1);
        bottom.transform.localScale = new Vector3(Mathf.Abs(rightTopCorner.x - leftBottomCorner.x), 1);
    }
}
