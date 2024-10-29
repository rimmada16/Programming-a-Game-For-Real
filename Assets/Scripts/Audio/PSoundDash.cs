using Character.Player;
using Unity.VisualScripting;

public class PSoundDash: PSoundBase
{
    void Start()
    {
        var BD = gameObject.GetComponent<DashSystem>();
        if (BD != null)
        {
            BD.OnDash+=PlaySound;
        }
        
    }
}