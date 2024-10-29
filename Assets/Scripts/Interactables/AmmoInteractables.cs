using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoInteractables : Interactable
{
    protected override void interact(Collider other)
    {
        base.interact(other);
        // Add to the Kunai count
        other.GetComponentInParent<PlayerProjectile>().ChangeAmmoBy(4);
        //Debug.Log(player.GetComponent<PlayerProjectile>().kunaiAmmo);

    }

}