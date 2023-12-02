using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SlashUnit : MonoBehaviour
{
    private LineRenderer lineRend;
    private bool lineRendGot;
    
    private Vector3[] linePositions;

    [SerializeField]
    private float maxLifetime = 1;
    private float timeRemaining;

    private void Start()
    {
        
        if (!lineRendGot)
        {
            lineRend = GetComponent<LineRenderer>();
            lineRend.enabled = false;
            lineRendGot = true;
        }
    }

    void Update()
    {
        if (lineRend.enabled)
        {
            
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Clamp(timeRemaining, 0, maxLifetime);
            
            float lerpFactor = timeRemaining / maxLifetime;
            
            var newCol = Color.Lerp(Color.clear, Color.white, lerpFactor);


            lineRend.startColor = newCol;
            lineRend.endColor = newCol;

            if (timeRemaining == 0)
            {
                EndSlash();
            }
            
            
        }
    }

    public void DoSlash(Vector3[] positions)
    {
        if (!lineRendGot)
        {
            lineRend = GetComponent<LineRenderer>();
            lineRendGot = true;
        }
        
        SlashManager.Instance.MoveSlashToActive(this);
        
        lineRend.enabled = true;
        timeRemaining = maxLifetime;
        
        lineRend.positionCount = positions.Length;
        lineRend.SetPositions(positions);
        
    }

    private void EndSlash()
    {
        lineRend.enabled = false;
        SlashManager.Instance.MoveSlashToInactive(this);
    }
}

