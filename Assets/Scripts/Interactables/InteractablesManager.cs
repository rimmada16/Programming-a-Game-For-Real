using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Interactables
{
    /// <summary>
    /// Handles the spawning of interactable objects
    /// </summary>
    public class InteractablesManager : Singleton<InteractablesManager>
    {
        private int _randomiserChoice;
        [SerializeField] private List<DroppableItem> droppableItems;
        [SerializeField] private float[] decimalChance;

        private GameObject _chosenItem;
        private Transform _currentSpawnTarget;

        /// <summary>
        /// Data structure for the droppable items
        /// </summary>
        [System.Serializable]
        public class DroppableItem
        {
            public GameObject item;
            public float weight;
        }

        /// <summary>
        /// Will produce a random item from the list of droppable items using the weight of each item
        /// </summary>
        /// <param name="atTransform">The location of the enemy/crate that was destroyed/killed</param>
        /// <returns></returns>
        public GameObject ProduceRandomItem(Transform atTransform)
        {
            if (droppableItems.Count == 0)
            {
                Debug.Log("We have no droppable items in the list");
                return null;
            }
            
            var totalWeight = 0f;
            // Find the total weight of all the droppable items
            foreach (var droppableItem in droppableItems)
            {
             totalWeight += droppableItem.weight;   
            }
            
            var randomValue = Random.Range(0f, totalWeight);
            var weightSum = 0f;
            
            // Accumulates weights of droppable items until the random value is surpassed, then instantiates
            // and returns the selected item.
            foreach (var droppableItem in droppableItems)
            {
                weightSum += droppableItem.weight;
                
                if (randomValue < weightSum)
                {
                    return InteractableInstantiator(atTransform, droppableItem.item);
                }
            }

            return null;
        }
        
        private GameObject InteractableInstantiator(Transform spawnLocation, GameObject itemToInstantiate)
        {
            var newObject = Instantiate(itemToInstantiate, spawnLocation.position, Quaternion.Euler(0, Random.Range(0f, 360f), 0));
            newObject.transform.parent = transform;
            return newObject;
        }
    }
}
