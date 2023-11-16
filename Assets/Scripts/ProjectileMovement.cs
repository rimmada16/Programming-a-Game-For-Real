using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int hardcoreDamage;
    [SerializeField] private int knockback;
    public Transform source;
    private GameObject HardcoreCheckerGO;
    private int originalDamage;
    
    
    public float speed = 10f;

    // Projectile lifetime length -- Set to 1 second
    public float lifetime = 1;
    
    // Start is called before the first frame update
    private void Start()
    {
        // Will throw an error if it is not active - Tried implementing try catch but couldn't get it to work
        HardcoreCheckerGO = GameObject.FindWithTag("HardcoreChecker");
        
        originalDamage = damage;
    }


    private void Awake()
    {
        // On Instantiate, destroy the gameobject after lifetime 
        Destroy(gameObject, lifetime);
        //Debug.Log("The projectile lifetime ran out");
    }

    // Update is called once per frame
    void Update()
    
    {
        // Transforms the Projectile
        var transform1 = transform;
        transform1.position += transform1.forward * (Time.deltaTime * speed);
        
        // Hardcore
        if (HardcoreCheckerGO.activeSelf)
        {
            Debug.Log("HARDCORE CHECKER IS ACTIVE");
            damage = hardcoreDamage;
        }
        else
        {
            damage = originalDamage;
        }
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == gameObject.layer)
        {
            return;
        }

        string[] layersWanted = { "Player", "Enemy", "Prop" };
        int otherLayer = (int) Mathf.Pow(2,other.gameObject.layer) ;
        if (otherLayer != LayerMask.GetMask(layersWanted[0])&& 
            otherLayer != LayerMask.GetMask(layersWanted[1])&&
            otherLayer != LayerMask.GetMask(layersWanted[2]))
        {
            //Debug.Log("hit something on wrong layer");
            
            Destroy(gameObject);
            return;
        }

        var HU = other.GetComponent<HealthUnit>();
        if ( HU == null)
        {
            Debug.Log("hit something without a health unit");
            return;
        }
        Debug.Log("hit success");
        HU.TakeDamage(damage, knockback, source);
        Destroy(gameObject);
        
    }
    
}

