using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource backgroundTrack;
    void Start()
    {
        backgroundTrack.Play();
    }
}
