using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAndSpin : MonoBehaviour
{
    [SerializeField] private float hoverMin, hoverMax, hoverSpeed;
    [SerializeField] private float spinSpeed;

    private float startTime;

    private Vector3 baseLocalPos;
    
    private Vector3 baseLocalEuler;

    private Transform myTransform;
    
    // Start is called before the first frame update
    void Awake()
    {
        myTransform = transform;
        
        startTime = Time.time;
        baseLocalPos = myTransform.localPosition;
        baseLocalEuler = myTransform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

        myTransform.localPosition = baseLocalPos + new Vector3(0, Mathf.Lerp(hoverMin, hoverMax, (Mathf.Sin((Time.time * hoverSpeed) +startTime)/2) +.5f), 0);
        myTransform.localEulerAngles = baseLocalEuler + new Vector3(0, (Time.time * spinSpeed) +startTime, 0);
    }
}
