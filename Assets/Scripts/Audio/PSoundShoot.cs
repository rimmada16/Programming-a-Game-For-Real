using Unity.VisualScripting;

public class PSoundShoot: PSoundBase
{
    void Start()
    {
        var PP = gameObject.GetComponent<PlayerProjectile>();
        if (PP != null)
        {
            PP.OnShoot+=PlaySound;
        }
        
    }
}