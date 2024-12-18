using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class BarDisplay : MonoBehaviour
{

    private List<Transform> barSegments;
    
    
    public int segments;
    public float offset;
    public float rotation;
    public float scale;


    public Sprite image;
    public int segmentsShown;

    public GameObject referenceSegment;
    
    public ValueGrabber value;

    
    // Start is called before the first frame update
    void Awake()
    {
        barSegments = new List<Transform>();
        UpdateBar();
        segmentsShown = segments;
        
        
        if (value != null)
        {
            value.OnValueUpdate += UpdateValue;
        }
    }


    private void Update()
    {
        //UpdateSegmentDisplay();
    }

    private void UpdateValue()
    {
        segmentsShown = value.GetValueI();
        //Debug.Log("Then Value coming through is "+value.GetValueI());
        //Debug.Log("Then Value coming through is float "+value.GetValueF());
            UpdateSegmentDisplay();
    }
    
    void UpdateSegmentDisplay()
    {
        segmentsShown = Mathf.Clamp(segmentsShown,0, segments);
        
        foreach (var segment in barSegments)
        {
            segment.GameObject().SetActive(true);
        }

        if (segmentsShown < segments)
        {
            barSegments[segmentsShown].GameObject().SetActive(false);
        }
        
    }



    void UpdateBar()
    {
        
        if (referenceSegment == null)
        {
            return;
        }

        for (int i = 0; i < segments; i++)
        {
            GameObject newSegment;
            if (i == 0)
            {
                newSegment = Instantiate(referenceSegment, parent:transform, worldPositionStays:true);
                newSegment.transform.localEulerAngles = new Vector3(0, 0, 0);
                newSegment.transform.localPosition = Vector3.zero;
            }
            else
            {
                newSegment = Instantiate(referenceSegment, parent:barSegments[i-1], worldPositionStays:true);
                newSegment.transform.localEulerAngles = new Vector3(0, 0, rotation);
                newSegment.transform.localPosition = new Vector3(offset*Mathf.Cos(rotation*Mathf.Deg2Rad)+offset,offset*Mathf.Sin(rotation*Mathf.Deg2Rad));
            }
            
            newSegment.transform.localScale = new Vector3(1, 1, 1);
            var newImage = newSegment.GetComponent<Image>();
            newImage.sprite = image;
            
            

            barSegments.Add(newSegment.transform);
        }
        
        
    }

}
