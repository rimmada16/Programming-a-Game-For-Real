using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class InteractablesManager : Singleton<InteractablesManager>
{
    private int _randomiserChoice;
    [SerializeField] private GameObject[] droppableItems;
    [SerializeField] private float[] decimalChance;

    private GameObject chosenItem;
    private Transform currentSpawnTarget;
    
    public GameObject ProduceRandomItem(Transform atTransform)
    {
        //get random number within length of array
        Randomiser();

        //assign item as that item
        chosenItem = droppableItems[_randomiserChoice];

        //sets the target transform location
        currentSpawnTarget = atTransform;

        //check if the same index has an assigned percent chance of being dropped
        if (decimalChance.Length >= _randomiserChoice + 1)
        {
            //makes sure the drop chance is within the correct range
            float dropChance = decimalChance[_randomiserChoice];
            dropChance = Mathf.Clamp01(dropChance);
            
            //randomly generates a number to decide whether drop will succeed
            float dropRandom = Random.Range(0f, 1f);
            Debug.Log("randomiser random"+dropRandom);

            //succeeds if rolls a lower number than %
            if (dropChance >= dropRandom)
            {
                return InteractableInstantiator();
            }
            else
            {
                return null;
            }
        }

        //auto succeed if it doesnt have an assigned percent value
        else
        {
            return InteractableInstantiator();
        }
        
             
    }

    private GameObject InteractableInstantiator()
    {
        var newObject = Instantiate(chosenItem, currentSpawnTarget.position, currentSpawnTarget.rotation);
        newObject.transform.parent = transform;

        return newObject;
    }

    private void Randomiser()
    {
        // Currently guaranteed to spawn health pickup
        //_randomiserChoice = Random.Range(minVal, maxVal);
        
        //random number of anything in the droppable items
        _randomiserChoice = Random.Range(0, droppableItems.Length );
        Debug.Log("randomiser chose"+_randomiserChoice);
    }
}
