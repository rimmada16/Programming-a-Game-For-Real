using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Projectile : MonoBehaviour
{
    public GameObject MovingProjectile;
    private Rigidbody _rb;
    public float projectileSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // For the player
        if (Input.GetMouseButtonDown(1))
        {
            // Spawns the Kunai 2m in front of the player
            Instantiate(MovingProjectile, transform.position + (transform.forward * 2), transform.rotation);
            MovingProjectile.name = "MovingProjectile";
            Debug.Log("Right Click was pressed");
        }

    }

}
