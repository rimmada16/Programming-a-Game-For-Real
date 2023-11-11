using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractablesManager : MonoBehaviour
{
    private int _randomiserChoice;
    [SerializeField] private int randMinVal;
    [SerializeField] private int randMaxVal;
    
    // For the HP Pickup
    [SerializeField] private GameObject healthInteractablePrefab;
    private GameObject healthInteractableGameObject;
    
    // For the Ammo Pickup
    [SerializeField] private GameObject ammoInteractablePrefab;
    private GameObject ammoInteractableGameObject;

    // Condenser 
    private GameObject chosenInteractableGameObject;
    private GameObject chosenInteractablePrefab;

    // Update is called once per frame
    void Update()
    {
        Randomiser();
        
        if (_randomiserChoice == 1)
        {
            chosenInteractableGameObject = healthInteractableGameObject;
            chosenInteractablePrefab = healthInteractablePrefab;
            InteractableInstantiator();
        }
        
        if (_randomiserChoice == 2)
        {
            chosenInteractableGameObject = ammoInteractableGameObject;
            chosenInteractablePrefab = ammoInteractablePrefab;
            InteractableInstantiator();
        }
        
        else
        {
            Debug.Log(_randomiserChoice + " - Spawned Nothing");
            Destroy(gameObject);
        }

        // Add in more Interactables here
        
             
    }

    private void InteractableInstantiator()
    {
        chosenInteractableGameObject = Instantiate(chosenInteractablePrefab, transform.position, Quaternion.identity);
        chosenInteractableGameObject.transform.parent = transform;
        chosenInteractableGameObject.transform.parent = null;
        Debug.Log(_randomiserChoice);
        
        Destroy(gameObject);
    }

    private void Randomiser()
    {
        // Currently guaranteed to spawn health pickup
        //_randomiserChoice = Random.Range(minVal, maxVal);
        _randomiserChoice = Random.Range(randMinVal, randMaxVal);
    }
}
