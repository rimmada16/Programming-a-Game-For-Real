using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemOnDeath : MonoBehaviour
{

    public bool dropItem = true;
    public bool destroySelfOnDeath;

    void Start()
    {
        var HU = GetComponent<HealthUnit>();
        if (HU != null)
        {
            GetComponent<HealthUnit>().OnDeath += DropItem;
        }
    }

    void DropItem()
    {
        if (dropItem)
        {
            
            InteractablesManager.Instance.ProduceRandomItem(transform);
        }
        
        //get loot table drop
        if (destroySelfOnDeath)
        {
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        
        GetComponent<HealthUnit>().OnDeath -= DropItem;
        Destroy(gameObject);
    }
}
