using Unity.VisualScripting;

public class PSoundDeath: PSoundBase
{
    void Start()
    {
        var HU = gameObject.GetComponent<HealthUnit>();
        if (HU != null)
        {
            HU.OnDeath+=PlaySound;
        }
        
    }
}