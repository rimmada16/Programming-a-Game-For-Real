using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Serialization;

public class PRTCLOnDeath : PRTCLBase
{
    void Start()
    {
        var HU = gameObject.GetComponent<HealthUnit>();
        if (HU != null)
        {
            HU.OnDeath+=MakeParticles;
        }
        
    }

    private void MakeParticles()
    {
        MakeParticlesAt(transform.position);
    }
}
