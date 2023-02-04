using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerController : MonoBehaviour
{
    public GameObject playerPrefab;
    public string playerName = "Player_1";
    void Start()
    {
        Instantiate(playerPrefab, gameObject.transform.position, Quaternion.identity);
    }
}
