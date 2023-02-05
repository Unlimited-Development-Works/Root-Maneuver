using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSpawnerController : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject plantPrefab;
    public string playerName;
    public Color playerColor = Color.white;
    private GameObject spawnedPlant;
    private GameObject player;
    public TextMeshProUGUI boostText;
    void Start()
    {
        player = Instantiate(playerPrefab, gameObject.transform.position, Quaternion.identity);
        spawnedPlant = Instantiate(plantPrefab, gameObject.transform.position + (0.25f * Vector3.up), Quaternion.identity);
        player.GetComponent<PlayerMovement>().playerName = playerName;
        player.GetComponent<RootPlacementController>().playerName = playerName;
        player.GetComponent<RootPlacementController>().plantController = spawnedPlant.GetComponent<PlantController>();
        player.name = playerName;
        player.GetComponent<SpriteRenderer>().color = playerColor;
        if (playerName == "Player_2")
        {
            boostText.alignment = TextAlignmentOptions.TopRight;
        }
        
    }

    void Update()
    {
        bool canBoost = player.GetComponent<PlayerMovement>().canBoost;
        bool isBoosting = player.GetComponent<PlayerMovement>().isBoosting;
        if (canBoost && !isBoosting)
        {
            boostText.enabled = true;
        }
        else
        {
            boostText.enabled = false;
        }




        
    }
}
