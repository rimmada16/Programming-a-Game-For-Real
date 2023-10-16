using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHitbox : MonoBehaviour
{
    
    public delegate void CollideHandler(Collider otherCol);
    public event CollideHandler OnCollide;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        OnCollide?.Invoke(other);
    }
}
