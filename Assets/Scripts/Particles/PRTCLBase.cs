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


    protected void MakeParticlesAt(Vector3 pos)
    {
        Instantiate(particles, parent: null, position: pos, rotation: new Quaternion());

    }
    
}
