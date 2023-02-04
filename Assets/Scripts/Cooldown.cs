using UnityEngine;

public class Cooldown {
    private float seconds;
    private float secondsRemaining;

    public Cooldown(float periodInSeconds) {
        seconds = periodInSeconds;
    }

    public void Reset() {
        secondsRemaining = seconds;
    }

    public void Update(float periodInSeconds = 0f) {
        if (periodInSeconds != 0f && periodInSeconds != seconds) {
            seconds = periodInSeconds;
        }
        if (secondsRemaining > 0) {
            secondsRemaining -= Time.deltaTime;
        }
    }

    public bool Expired() {
        return secondsRemaining <= 0;
    }
}