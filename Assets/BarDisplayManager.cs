using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarDisplayManager : MonoBehaviour
{

    public List<BarDisplay> displaysSynced;

    public int segments;
    public float offset;
    public float rotation;
    
    public GameObject referenceSegment;

    
    // Start is called before the first frame update
    void Start()
    {
        UpdateBars();

    }

    
    /*
    private void OnValidate()
    {
        try
        {
            UpdateBars();
            
        }
        catch
        {
            
        }
    }*/

    void UpdateBars()
    {
        foreach (var bar in displaysSynced)
        {
            bar.segments = segments;
            bar.offset = offset;
            bar.rotation = rotation;
            bar.scale = transform.localScale.y;

            bar.referenceSegment = referenceSegment;
        }
    }
}
