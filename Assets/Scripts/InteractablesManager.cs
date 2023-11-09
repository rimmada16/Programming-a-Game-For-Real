using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractablesManager : MonoBehaviour
{
    private int randomiserChoice;
    [SerializeField] private GameObject healthInteractablePrefab;
    private GameObject healthInteractableGameObject;

    // Update is called once per frame
    void Update()
    {
        Randomiser();
        
        if (randomiserChoice == 1)
        {
            healthInteractableGameObject = Instantiate(healthInteractablePrefab, transform.position, Quaternion.identity);
            healthInteractableGameObject.transform.parent = transform;
            healthInteractableGameObject.transform.parent = null;
            Debug.Log(randomiserChoice + " - Spawned HP Interactable");
            
            Destroy(gameObject);
        }
        
        // Add in more Interactables here
        
        
        else
        {
            Debug.Log(randomiserChoice + " - Spawned Nothing");
            Destroy(gameObject);
        }
    }

    private void Randomiser()
    {
        // Currently guaranteed to spawn health pickup
        //randomiserChoice = Random.Range(minVal, maxVal);
        randomiserChoice = Random.Range(1, 1);
    }
}
