using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class valueGrabber : MonoBehaviour
{
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
        float newValue = Mathf.Lerp(outputMin, outputMax, inputMin / inputMax);
        return newValue;
    }
    
    public int GetValueI()
    {
        int newValue = Mathf.FloorToInt(Mathf.Lerp(outputMin, outputMax, inputMin / inputMax)) ;
        return newValue;
    }

    public void SetValue(float valueInput)
    {
        value = Mathf.Clamp(valueInput, inputMin, inputMax);
        Notify();
    }

}
