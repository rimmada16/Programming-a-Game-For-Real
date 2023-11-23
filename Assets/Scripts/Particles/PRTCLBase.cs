using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Serialization;

public class PRTCLBase : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem particles;

    
    // Start is called before the first frame update
    void Start()
    {
        //eventForSound += PlaySound;
    }


    protected void MakeParticles()
    {
        Instantiate(particles, parent: null, position: transform.position, rotation: new Quaternion());

    }
}
