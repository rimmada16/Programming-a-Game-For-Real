using Unity.VisualScripting;

public class PSoundSlashDud: PSoundBase
{
    void Start()
    {
        var MA = gameObject.GetComponent<MeleeAttacker>();
        if (MA != null)
        {
            MA.OnAttackDud+=PlaySound;
        }
        
    }
}