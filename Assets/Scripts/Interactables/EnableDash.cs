using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDash : Interactable
{
    [SerializeField] private GameObject targetUI;
    
    protected override void interact(Collider other)
    {
        if (!other.GetComponent<BasicDash>().enabled)
        {
            targetUI.SetActive(true);
            other.GetComponent<BasicDash>().enabled = true; 
        }
    }
}