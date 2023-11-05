using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AIBehaviour
{
    protected Transform me;
    protected Transform lookingDirection;

    public abstract void Update();

    public virtual void EnterBehaviour(Transform newMe, Transform newLookingDirection)
    {
        SetMe(newMe,newLookingDirection);
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
    
    // hard coded time - put into the editor if possible
    public float attackCooldownMaxT = 1.5f;
    private float attackCooldownCounter;
    
    private MeleeAttacker thisMeleeAttacker;
    private bool getAttackFailed = false;

    public override void EnterBehaviour(Transform newMe, Transform newLookingDirection)
    {
        base.EnterBehaviour(newMe, newLookingDirection);
        
        attackCooldownCounter = attackCooldownMaxT;
        thisMeleeAttacker = me.GetComponent<MeleeAttacker>();
        if (thisMeleeAttacker == null)
        {
            getAttackFailed = true;
        }
    }

    public override void Update()
    {
        if (getAttackFailed)
        {
            return;
        }
        
        if (attackCooldownCounter > 0)
        {
            attackCooldownCounter -= Time.deltaTime;
        }

        if (attackCooldownCounter <= 0)
        {
            attackCooldownCounter = attackCooldownMaxT;
            thisMeleeAttacker.Attack();
            
        }
    }
}

public class AIAttackProjectile : AIBehaviour
{
    // hard coded time - put into the editor if possible
    public float projectileCooldownMaxT = 1f;
    private float projectileCooldownCounter;
    
    public override void Update()
    
    {
        if (projectileCooldownCounter > 0)
        {
            projectileCooldownCounter -= Time.deltaTime;
        }

        if (projectileCooldownCounter <= 0)
        {
            projectileCooldownCounter = projectileCooldownMaxT;
            ProjectileManager.Instance.MakeProjectileAt(me.gameObject, lookingDirection, 0); 
        }
        
    }
}

public class AIPatrol : AIBehaviour
{
    public override void Update()
    {
        
    }
}