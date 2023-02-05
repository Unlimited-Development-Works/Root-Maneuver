using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour
{
    public Vector2 magnitude;

    private float magnitudeMultiplier = 1f;
    private float timeRemaining = 0f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (timeRemaining > 0f) {
            timeRemaining -= Time.deltaTime;
            transform.position = originalPosition + magnitudeMultiplier * new Vector3(
                Random.Range(-magnitude.x/2, magnitude.x/2),
                Random.Range(-magnitude.y/2, magnitude.y/2),
                0f
            );
        }    
    }

    public void Shake(float seconds, float magnitudeMultiplier = 1f) {
        timeRemaining = seconds;
        this.magnitudeMultiplier = magnitudeMultiplier;
    }
}
