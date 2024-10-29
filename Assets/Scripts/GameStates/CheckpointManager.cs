using System;
using System.Collections;
using System.Collections.Generic;
using Character.Player;
using UnityEngine;

public class CheckpointManager : Singleton<CheckpointManager>
{
    private static bool hasCheckpoint;
    private static Vector3 lastCheckpoint;
    private static Vector3 lastCheckpointRot;
    private static bool hasDash;
    private static bool hasKunai;

    
    
    public static void ResetToDefaults()
    {
        hasCheckpoint = false;
        lastCheckpoint = new Vector3();
        lastCheckpointRot = new Vector3();
        hasDash = false;
        hasKunai = false;
        EnemyGateManager.ResetGateData();
    }

    public void StoreNewCheckpoint(GameObject player, Transform newPos)
    {
        hasDash = player.GetComponent<DashSystem>().enabled;
        hasKunai = player.GetComponent<PlayerProjectile>().enabled;
        lastCheckpoint = newPos.position;
        lastCheckpointRot = newPos.eulerAngles;
        hasCheckpoint = true;
        EnemyGateManager.Instance.StoreGateData();
        
    }
    
    
    public void StartAtCheckpoint(GameObject player)
    {
        Debug.Log("lets see");
        if ( hasCheckpoint)
        {
            Debug.Log("Starting at checkpoint");
            player.GetComponent<DashSystem>().EnableSelf(hasDash); 
            player.GetComponent<PlayerProjectile>().EnableSelf(hasKunai);
            player.transform.position = lastCheckpoint;
            player.transform.eulerAngles = lastCheckpointRot;
        }

    }
}
