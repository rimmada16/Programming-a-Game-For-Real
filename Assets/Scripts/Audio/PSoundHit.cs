using Unity.VisualScripting;

public class PSoundHit: PSoundBase
{
    void Start()
    {
        var HU = gameObject.GetComponent<HealthUnit>();
        if (HU != null)
        {
            HU.OnHit+=PlaySound;
        }
        
    }
}