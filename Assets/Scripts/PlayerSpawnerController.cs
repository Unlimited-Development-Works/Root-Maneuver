using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerController : MonoBehaviour
{
    public GameObject playerPrefab;
    public string playerName = "Player_1";
    void Start()
    {
        GameObject player =Instantiate(playerPrefab, gameObject.transform.position, Quaternion.identity);
        player.GetComponent<PlayerMovement>().playerName = playerName;
        player.GetComponent<RootPlacementController>().playerName = playerName;
        player.name = playerName;

    }
}
