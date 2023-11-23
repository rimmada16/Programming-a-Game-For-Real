using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using https://gamedevbeginner.com/how-to-play-audio-in-unity-with-examples/;
// using https://www.youtube.com/watch?v=p8KswsmGlpc;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private List<AudioClip> clips;

    // For the music
    [SerializeField] private AudioSource playerAudioSource;
    
    [SerializeField] public float volume = 0.5f;
    
    
    
    // Sound effects at position, example being when an enemy dies
    public void PlaySoundAtPoint(int index, Transform targetTransform)
    {
        
        AudioSource.PlayClipAtPoint(clips[index], targetTransform.position, volume);
    }

    // Music
    public void Play(int index)
    {
        playerAudioSource.clip = clips[index];
        playerAudioSource.volume = volume;
        playerAudioSource.Play();
    }
}