using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AIBehaviour
{
    protected Transform me;
    protected Transform lookingDirection;
    public abstract void Update();

    public virtual void EnterBehaviour()
    {
        Debug.Log(this.GetType().Name + " enter");
    }
    public virtual void ExitBehaviour()
    {
        
        Debug.Log(this.GetType().Name + " exit");
    }

    public void SetMe(Transform newMe, Transform newLooking)
    {
        me = newMe;
        lookingDirection = newLooking;
    }
}

public class AIIdle : AIBehaviour
{
    public override void Update()
    {
        
    }
}


public class AIApproach : AIBehaviour
{
    private float speed;
    private Transform target;
    private bool moveBackwards;

    public AIApproach(Transform newTarget, float newSpeed, bool movingBackwards = false)
    {
        speed = newSpeed;
        target = newTarget;
        moveBackwards = movingBackwards;
    }
    
    public override void Update()
    {
        //Debug.Log("want to move");
        //check if not a wall in the way of approaching
        
        //apply a movement distance on whatever this script is attached to
        if (me != null)
        {
            var distToMove = GetTargetDirection() * speed * Time.deltaTime;
            me.position += (Vector3) distToMove;
            
            //Debug.Log("moving by "+ distToMove);
        }
    }
    public override void EnterBehaviour()
    {
        //Debug.Log(this.GetType().Name + " enter at speed "+ speed);
    }

    public Vector3 GetTargetDirection()
    {
        Vector3 newDir =  target.position - me.position;

        newDir.y = 0;
        newDir = newDir.normalized;
        return(newDir);
    }

}


public class AIAttackMelee : AIBehaviour
{
    public override void Update()
    {
        
    }
}

public class AIAttackProjectile : AIBehaviour
{
    public override void Update()
    {
        
        ProjectileManager.Instance.MakeProjectileAt(me.gameObject, lookingDirection, 0);
    }
}

public class AIPatrol : AIBehaviour
{
    public override void Update()
    {
        
    }
}