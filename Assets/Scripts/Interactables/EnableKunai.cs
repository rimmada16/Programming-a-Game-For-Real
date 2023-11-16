using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableKunai : Interactable
{
    [SerializeField] private GameObject ammoBarUI;
    [SerializeField] private GameObject projectileCooldownBarUI;
    
    protected override void interact(Collider other)
    {
        if (!other.GetComponent<PlayerProjectile>().enabled)
        {
            ammoBarUI.SetActive(true);
            projectileCooldownBarUI.SetActive(true);
            other.GetComponent<PlayerProjectile>().enabled = true; 
        }
    }
}