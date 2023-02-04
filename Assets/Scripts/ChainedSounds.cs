using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainedSounds : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public List<AudioClip> clips = new List<AudioClip>();
    public float crossFadePeriod = 0.1f;
    public float fadeOutRate = 1f;
    private int currentClipIndex = 0;
    private int currentSourceIndex = 0;
    private AudioSource[] audioSources;
    private float timeRemaining = 0f;

    void Start()
    {
        audioSources = new AudioSource[] {
            audioSource1,
            audioSource2
        };
    }

    public bool IsPlaying() {
        return (CurrentSource().isPlaying || NextSource().isPlaying);
    }

    public void PlayFor(float seconds) {
        if (IsPlaying()) {
            timeRemaining += seconds;
        } else {
            timeRemaining = seconds;
        }
    }

    public void Stop() {
        timeRemaining = 0f;
    }

    void Update()
    {
        if (!audioSource1 || !audioSource2) {
            Debug.LogError("Need to assign 2 audio sources");
            return;
        }
        if (timeRemaining > 0) {
            if (!CurrentSource().isPlaying && !NextSource().isPlaying) {
                // Start playing
                CurrentSource().volume = 1;
                CurrentSource().clip = clips[currentClipIndex];
                CurrentSource().Play();
            } else if (CurrentSource().isPlaying && !NextSource().isPlaying) {
                if (CurrentSource().clip.length - CurrentSource().time < crossFadePeriod) {
                    NextSource().clip = clips[NextClipIndex()];
                    NextSource().volume = 0;
                    NextSource().Play();
                }
            } else if (!CurrentSource().isPlaying) {
                AdvanceSource();
                CurrentSource().volume = 1f;
                AdvanceClipIndex();
            }
            if (NextSource().isPlaying) {
                if (NextSource().time < crossFadePeriod) {
                    NextSource().volume = NextSource().time / crossFadePeriod;
                } else {
                    NextSource().volume = 1f;
                }
            }
        } else {
            FadeOutSource(CurrentSource());
            FadeOutSource(NextSource());
        }
    }

    private void FadeOutSource(AudioSource source) {
        if (source.isPlaying) {
            if (source.volume > 0f) {
                source.volume = Mathf.Max(source.volume - fadeOutRate * Time.deltaTime, 0f);
            } else {
                source.Stop();
            }
        }
    }

    private AudioSource CurrentSource() {
        return audioSources[currentSourceIndex];
    }

    private AudioSource NextSource() {
        return audioSources[(currentSourceIndex + 1) % audioSources.Length];
    }

    private void AdvanceSource() {
        currentSourceIndex = (currentSourceIndex + 1) % audioSources.Length;
    }

    private int NextClipIndex() {
        return (currentClipIndex + 1) % clips.Count;
    }

    private void AdvanceClipIndex() {
        currentClipIndex = NextClipIndex();
    }
}
