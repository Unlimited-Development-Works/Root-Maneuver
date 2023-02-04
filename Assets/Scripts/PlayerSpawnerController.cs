using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerController : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public string PlayerName = "Player_1";
    void Start()
    {
        Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
