using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AIBehaviour
{
    public abstract void Update();

    public virtual void EnterBehaviour()
    {
        Debug.Log(this.GetType().Name + " enter");
    }
    public virtual void ExitBehaviour()
    {
        
        Debug.Log(this.GetType().Name + " exit");
    }
}

public class AIIdle : AIBehaviour
{
    public override void Update()
    {
        
    }
}


public class AIAlerted : AIBehaviour
{
    public override void Update()
    {
        
    }

    public void ReadDistanceToTarget()
    {
        
    }
}