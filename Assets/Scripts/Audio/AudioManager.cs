using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using https://gamedevbeginner.com/how-to-play-audio-in-unity-with-examples/;
// using https://www.youtube.com/watch?v=p8KswsmGlpc;

public class AudioManager : Singleton<AudioManager>
{
    // For the music
    [SerializeField] public AudioSource playerAudioSource;
    [SerializeField] public static float volume = 0.5f;

    private void Awake()
    {
        VolumeSlider.Instance.ChangeVolume(volume);
    }
    
}
