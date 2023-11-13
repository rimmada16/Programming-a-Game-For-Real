


using UnityEngine;

public class Interactable: MonoBehaviour
{
    protected bool destroyOnContact;
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag( "Player"))
        {
            interact(other);
            TryDestroy();
        }
    }

    protected virtual void interact(Collider other)
    {

    }

    protected void TryDestroy()
    {
        if (destroyOnContact)
        {
            Destroy(gameObject);
        }
    }
}