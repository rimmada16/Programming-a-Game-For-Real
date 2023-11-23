using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Serialization;

public class PRTCLOnHit : PRTCLBase
{
    void Start()
    {
        var HU = gameObject.GetComponent<HealthUnit>();
        if (HU != null)
        {
            HU.OnHit+=MakeParticles;
        }
        
    }

}
