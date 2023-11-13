


using UnityEngine;

public class Interactable: MonoBehaviour
{
    [SerializeField] protected bool destroyOnContact = true;
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