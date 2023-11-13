using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthInteractable : Interactable
{
    [SerializeField] private int heal;

    
    protected override void interact(Collider other)
    {
        HealPlayer(other);
    }
    
    public void HealPlayer( Collider other)
    {
        // Check if the player GameObject has the HealthUnit component attached.
        HealthUnit healthUnit = other.GetComponent<HealthUnit>();
        if (healthUnit != null)
        {
            // Do the heal + Nuke the object
            Debug.Log("Healed the player");
            healthUnit.GetHealed(heal);
            Destroy(gameObject);
        }
    }
}
