using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    GameObject MovingProjectile;
    GameObject MovingProjectileClone;

    [SerializeField] private int Damage;

    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.name == "MovingProjectile(Clone)")
        {
            AttackCollided(collision.collider);
            // Destroys the projectile that has been created when it collides with anything
            Destroy(gameObject);
        }

    }
    void AttackCollided(Collider otherCol)
    {
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
        otherHU.TakeDamage(Damage);
    }
}