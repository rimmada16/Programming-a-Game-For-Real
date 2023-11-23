using Unity.VisualScripting;

public class PSoundHeal: PSoundBase
{
    void Start()
    {
        var HU = gameObject.GetComponent<HealthUnit>();
        if (HU != null)
        {
            HU.OnHeal+=PlaySound;
        }
        
    }
}