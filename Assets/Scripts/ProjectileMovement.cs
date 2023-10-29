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
        // Transforms the Projectile
        var transform1 = transform;
        transform1.position += transform1.forward * (Time.deltaTime * speed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit the player");
        }
        // If the parent of the projectiles name is Player this will run:
        if (transform.parent != null && transform.parent.gameObject.name == "Player")
        {
            PlayerProjectile(collision.collider);
            // Destroys the projectile that has been created when it collides with anything
            Destroy(gameObject);
        }
        // If the parent of the projectiles name is anything other than Player this will run:
        else
        {
            EnemyProjectile(collision.collider);
            Destroy(gameObject);
        }
    }

    // Player projectile shenanigans 
    private void PlayerProjectile(Collider collisionTwo)
    {
        Debug.Log("PlayerProjectile ran");
        // Sorts out the dmg dealt when the Projectile collides with something that can take dmg
        // collisionTwo.gameObject.name == "Player" -- Friendly fire check
        if (collisionTwo.gameObject.layer == gameObject.layer || collisionTwo.gameObject.layer == LayerMask.GetMask("Terrain") || collisionTwo.gameObject.name == "Player")
        {
            return;
        }

        HealthUnit otherHU = collisionTwo.GetComponent<HealthUnit>();
        if (otherHU == null)
        {
            return;
        }
        otherHU.TakeDamage(damage);
    }
    
    // Enemy projectile shenanigans
    private void EnemyProjectile(Collider collisionThree)
    {
        Debug.Log("EnemyProjectile ran");
        // Sorts out the dmg dealt when the Projectile collides with something that can take dmg
        // collisionThree.gameObject.layer == LayerMask.GetMask("Enemy") -- Friendly fire check -- For all enemies apply the "Enemy" Layer
        if (collisionThree.gameObject.layer == gameObject.layer || collisionThree.gameObject.layer == LayerMask.GetMask("Terrain") || collisionThree.gameObject.layer == LayerMask.GetMask("Enemy"))
        {
            return;
        }

        HealthUnit otherHU = collisionThree.GetComponent<HealthUnit>();
        if (otherHU == null)
        {
            return;
        }
        otherHU.TakeDamage(damage);
    }
    
}