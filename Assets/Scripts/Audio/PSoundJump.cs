using Unity.VisualScripting;

public class PSoundJump: PSoundBase
{
    void Start()
    {
        var PC = gameObject.GetComponent<PlayerController>();
        if (PC != null)
        {
            PC.OnJump+=PlaySound;
        }
        
    }
}