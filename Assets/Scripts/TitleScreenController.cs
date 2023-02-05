using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
       void Update()
    {
        if (Input.GetButtonDown("Reset"))
        {
            SceneManager.LoadScene("3_player");
        }
    }
}
