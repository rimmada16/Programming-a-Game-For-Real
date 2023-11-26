using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGate : MonoBehaviour
{
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
        EnemyGateManager.Instance.OnEnemyKilled();
    }
    
}
