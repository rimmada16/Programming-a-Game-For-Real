using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private int damage;

    public float speed = 10f;

    // Projectile lifetime length -- Set to 1 second
    public float lifetime = 1;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        // On Instantiate, destroy the gameobject after lifetime 
        Destroy(gameObject, lifetime);
        Debug.Log("The projectile lifetime ran out");
    }

    // Update is called once per frame
    void Update()
    
    {
        // Transforms the Projectile in a direct straight path -- Does not take into account verticality 
        transform.position += transform.forward * Time.deltaTime * speed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        AttackCollided(collision.collider);
        // Destroys the projectile that has been created when it collides with anything
        Destroy(gameObject);
    }
    void AttackCollided(Collider otherCol)
    {
        Debug.Log("AttackCollided ran");
        // Sorts out the dmg dealt when the Projectile collides with something that can take dmg
        if (otherCol.gameObject.layer == gameObject.layer)
        {
            return;
        }

        if (otherCol.gameObject.layer == LayerMask.GetMask("Terrain"))
        {
            return;
        }

        HealthUnit otherHU = otherCol.GetComponent<HealthUnit>();
        if (otherHU == null)
        {
            return;
        }
        otherHU.TakeDamage(damage);
    }
}