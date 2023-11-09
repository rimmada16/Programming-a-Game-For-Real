using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthInteractable : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private int heal;

    private void Update()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            HealPlayer(heal);
        }
    }
    
    public void HealPlayer(int heal)
    {
        // Check if the player GameObject has the HealthUnit component attached.
        HealthUnit healthUnit = player.GetComponent<HealthUnit>();
        if (healthUnit != null)
        {
            // Call the GetHealed method to heal the player.
            Debug.Log("Healed the player");
            healthUnit.GetHealed(heal);
            Destroy(gameObject);
        }
    }
}
