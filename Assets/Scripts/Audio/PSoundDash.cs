using Unity.VisualScripting;

public class PSoundDash: PSoundBase
{
    void Start()
    {
        var BD = gameObject.GetComponent<BasicDash>();
        if (BD != null)
        {
            BD.OnDash+=PlaySound;
        }
        
    }
}