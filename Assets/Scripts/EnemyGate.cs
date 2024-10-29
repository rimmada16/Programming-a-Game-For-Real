using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGate : MonoBehaviour
{
    [SerializeField] private int gateNumber = -1;
    
    void Start()
    {
        var HU = GetComponent<HealthUnit>();
        if (HU != null)
        {
            GetComponent<HealthUnit>().OnDeath += EnemyDeath;
        }
    }

    private void OnDisable()
    {
        GetComponent<HealthUnit>().OnDeath -= EnemyDeath;
    }

    private void EnemyDeath()
    {
        
        EnemyGateManager.Instance.RegisterEnemyDeathAtGate(gateNumber);
    }
    
}
