using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInteractables : Interactable
{
    protected override void interact(Collider other)
    {
        base.interact(other);
        // Add to the Kunai count
        other.GetComponent<PlayerProjectile>().ChangeAmmoBy(4);
        //Debug.Log(player.GetComponent<PlayerProjectile>().kunaiAmmo);

    }

}