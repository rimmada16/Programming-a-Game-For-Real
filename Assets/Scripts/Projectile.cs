using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Projectile : MonoBehaviour
{
    public GameObject MovingProjectile;
    private Rigidbody _rb;
    public string projectileName;
    private float projectileCooldownCounter;
    // Sets the maximum cooldown for the projectile in the editor
    public float projectileCooldownMax;

    [SerializeField] Transform referenceDirection;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Decrements the cooldown for the projectile
        // This could possibly be altered by upgrades / power-ups
        if (projectileCooldownCounter > 0)
        {
            projectileCooldownCounter -= Time.deltaTime;
        }
        
        // For the player
        if (Input.GetMouseButtonDown(1) && projectileCooldownCounter <= 0)
        {
            projectileCooldownCounter = projectileCooldownMax;
            // Spawns the projectile 2m in front of the player
            var newProjectile =  Instantiate(MovingProjectile, transform.position + (transform.forward * 0) + (transform.up * 0.75f), transform.rotation );

            if (referenceDirection != null)
            {
                newProjectile.transform.rotation = referenceDirection.rotation;
            }
            MovingProjectile.name = projectileName;
            Debug.Log("Right Click was pressed");
        }

    }

}
