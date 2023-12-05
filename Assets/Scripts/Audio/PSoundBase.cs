using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Serialization;

public class PSoundBase : MonoBehaviour
{
    [SerializeField]
    protected AudioClip audioClip;

    [SerializeField] private float localVolume = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        //eventForSound += PlaySound;
    }


    protected void PlaySound()
    {
        
        AudioSource.PlayClipAtPoint(audioClip, transform.position, localVolume * AudioManager.volume);
        //Debug.Log("sound played");
    }
}
