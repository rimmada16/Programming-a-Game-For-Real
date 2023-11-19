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
        other.GetComponent<PlayerProjectile>().EnableSelf(true);
    }
}