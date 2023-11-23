using Unity.VisualScripting;

public class PSoundInteract: PSoundBase
{
    void Start()
    {
        var inter = gameObject.GetComponent<Interactable>();
        if (inter != null)
        {
            inter.OnInteract+=PlaySound;
        }
        
    }
}