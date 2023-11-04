using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeController : MonoBehaviour
{
    private MeleeAttacker thisMeleeAttacker;

    private bool failedToFind = false;
    
    
    void Start()
    {
        thisMeleeAttacker = GetComponent<MeleeAttacker>();
        if (thisMeleeAttacker == null)
        {
            failedToFind = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance.isPaused)
        {
            return;
        }

        if (!failedToFind)
        {
            //if the cooldown is entirely down, another slash can run
            if (Input.GetMouseButtonDown(0))
            {
                thisMeleeAttacker.Attack();
            }
        }
        
    }
}
