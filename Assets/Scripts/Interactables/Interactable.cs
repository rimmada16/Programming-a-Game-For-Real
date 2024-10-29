


using UnityEngine;

public class Interactable: MonoBehaviour
{
    [SerializeField] protected bool destroyOnContact = true;
    
    public delegate void GeneralHandler();
    public event GeneralHandler OnInteract;
    
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interact(other);
            TryDestroy();
        }
    }

    protected virtual void interact(Collider other)
    {
        OnInteract?.Invoke();
    }

    protected void TryDestroy()
    {
        if (destroyOnContact)
        {
            Destroy(gameObject);
        }
    }
}