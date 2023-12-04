using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioManager audioManager;

    private void Start()
    {
        // Set the initial value of the slider to the current volume.
        volumeSlider.value = audioManager.volume;

        // Youtube subscription
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeVolume(float newVolume)
    {
        audioManager.volume = newVolume;
        audioManager.playerAudioSource.volume = newVolume;
    }
}