using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInteractables : MonoBehaviour
{                  
    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            // Add one to the Kunai count
            player = GameObject.Find("Player");
            player.GetComponent<PlayerProjectile>().kunaiAmmo++;
            //Debug.Log(player.GetComponent<PlayerProjectile>().kunaiAmmo);
            
            Destroy(gameObject);
        }
    }
}