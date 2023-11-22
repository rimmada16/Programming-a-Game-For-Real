using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Was getting lazy and couldn't be bothered to do the parkour every time

public class Dev : MonoBehaviour
{
    //Get Player
    [SerializeField] private GameObject thePlayer;

    //Dash
    [SerializeField] private GameObject targetUI;

    // Kunai
    [SerializeField] private GameObject ammoBarUI;
    [SerializeField] private GameObject projectileCooldownBarUI;

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            // Dash
            targetUI.SetActive(true);
            thePlayer.GetComponent<BasicDash>().EnableSelf(true);

            // Kunai
            ammoBarUI.SetActive(true);
            projectileCooldownBarUI.SetActive(true);
            thePlayer.GetComponent<PlayerProjectile>().EnableSelf(true);
        }
    }
}
