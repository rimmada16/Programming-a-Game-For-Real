using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueGrabber : MonoBehaviour
{
    [SerializeField] 
    private float value;

    [SerializeField] private float inputMin, inputMax, outputMin, outputMax;
    
    
    
    public delegate void ChangeValueHandler();
    public event ChangeValueHandler OnValueUpdate;

    public void Notify()
    {
        
        OnValueUpdate?.Invoke();
    }

    public float GetValueF()
    {
        float t = Mathf.InverseLerp(inputMin, inputMax, value);
        float newValue = Mathf.Lerp(outputMin, outputMax, t);
        return newValue;
    }
    
    public int GetValueI()
    {
        float t = Mathf.InverseLerp(inputMin, inputMax, value);
        float newValue = Mathf.Lerp(outputMin, outputMax, t);
        //Debug.Log("mapped value is "+ newValue);
        return Mathf.FloorToInt(newValue);
    }

    public void SetInputMinMax(float min, float max)
    {
        inputMin = min;
        inputMax = max;
    }

    public void SetValue(float valueInput)
    {
        Debug.Log("Value coming through is "+valueInput);
        if (inputMin < inputMax)
        {
            value = Mathf.Clamp(valueInput, inputMin, inputMax);
        }
        else
        {
            value = Mathf.Clamp(valueInput, inputMax, inputMin);
        }
        Debug.Log("value changed to "+valueInput+" but actually "+value);
        Notify();
    }

}
