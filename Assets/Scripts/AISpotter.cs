using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AISpotter : MonoBehaviour
{
    private Transform[] allTarget;
    [SerializeField] private Transform eyes;
    [SerializeField] private float distance;
    [SerializeField] private float dotProductAngle;
    [SerializeField] private LayerMask opaqueLayers;

    private RaycastHit hitInfo;


    private void Start()
    {
        try
        {
            allTarget = PlayerTargetsManager.Instance.GetAllTargets();
        }
        catch
        {
            allTarget = null;
            Debug.LogWarning("Enemy failed to find Player targets");
        }
        
    }

    public Transform TryToSpotTargets()
    {
        //Debug.Log("number of targets to check: "+allTarget.Length);
        bool[] hitTargets = new bool[allTarget.Length];
        
        
        //checks each target on player if it can be seen
        for (int i = 0; i < allTarget.Length; i++)
        {
            Vector3 directionToTarget = allTarget[i].position - eyes.position;

            //checks if its too far to not waste resources
            if (directionToTarget.magnitude > distance)
            {
                
                //Debug.Log("target number "+i+" too far, distance is "+ directionToTarget.magnitude);
                continue;
            }

            directionToTarget = directionToTarget.normalized;
            
            //checks if its outside the vision cone to not waste resources
            if (Vector3.Dot(directionToTarget, eyes.forward) < dotProductAngle)
            {
                //Debug.Log("target number "+i+" out of vision cone, dot is "+ Vector3.Dot(directionToTarget, eyes.forward));
                continue;

            }
            
            //store results of raycast towards player
            // var count = Physics.RaycastNonAlloc(origin: eyes.position,direction: directionToTarget, results: results, maxDistance: distance, layerMask: opaqueLayers);
            //Physics.RaycastNonAlloc(origin: eyes.position,direction: directionToTarget, results: results, maxDistance: distance, );

            if (Physics.Raycast(eyes.position, directionToTarget, out hitInfo, distance, opaqueLayers))
            {
                if (hitInfo.collider.CompareTag("Player"))
                {
                    return (allTarget[i]);
                }
            }
            
            //Array.Sort(results, (hit, raycastHit) => raycastHit.distance.CompareTo(hit.distance));
            
            //checks each result of the raycast
            // for (int j = 0; j < results.Length; j++)
            // {
            //     try
            //     {
            //         if (results[j].transform.gameObject.layer == LayerMask.NameToLayer ("Player"))
            //         {
            //             Debug.Log("Hit player");
            //             return (allTarget[i]);
            //         }
            //         else
            //         {
            //             //######################################## trying to figure out why raycast goes through player ###############################################################
            //             Debug.Log("Hit something on layer type " + results[j].transform.gameObject.layer);
            //             placeholderLook.position = results[j].point;
            //         }
            //
            //     }
            //     catch
            //     {
            //         Debug.LogWarning("Enemy looking for player raycast hit something without a proper game object");
            //         hitTargets[i] = false;
            //     }
            // }
            
        }
        
        return null;

    }

    public Transform GetFirstTarget()
    {
        return (allTarget[0]);
    }


}
