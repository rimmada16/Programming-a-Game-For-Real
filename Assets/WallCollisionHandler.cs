using UnityEngine;

public class WallCollisionHandler : MonoBehaviour
{
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the tag "Wall"
        if (collision.gameObject.CompareTag("Wall"))
        {
            // If it's a wall, set the velocity of the Rigidbody to zero to lose all momentum
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            string collidedObjectName = gameObject.name;
            Debug.Log("<color=#FF69B4>The prop " + collidedObjectName + " has collided with a wall</color>");
        }
    }
}