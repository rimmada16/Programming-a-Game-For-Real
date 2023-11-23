using Unity.VisualScripting;

public class PSoundSlash: PSoundBase
{
    void Start()
    {
        var MA = gameObject.GetComponent<MeleeAttacker>();
        if (MA != null)
        {
            MA.OnAttackSuccess+=PlaySound;
        }
        
    }
}