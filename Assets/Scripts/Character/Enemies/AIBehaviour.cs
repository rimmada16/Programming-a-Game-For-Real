using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class AIBehaviour
{
    protected Transform me;
    protected Transform myFace;

    protected Transform myTarget;

    protected bool busy;
    protected bool isStuck;
    
    public abstract void Update();

    public virtual void EnterBehaviour(Transform newMe, Transform newFace, Transform target)
    {
        SetMe(newMe,newFace);
        myTarget = target;
        //Debug.Log(this.GetType().Name + " enter");
    }

    public bool IsBusy()
    {
        return busy;
    }
    
    public bool IsStuck()
    {
        return isStuck;
    }

    public virtual void ExitBehaviour()
    {
        
        //Debug.Log(this.GetType().Name + " exit");
    }

    public void SetMe(Transform newMe, Transform newFace)
    {
        me = newMe;
        myFace = newFace;
    }

    protected void rotateToLookAt(Vector3 lookTarget)
    {
        if ( me != null && myFace != null)
        {
            Vector3 v_diff;
            float atan2;
            
            v_diff = (lookTarget - me.position);	
            atan2 = Mathf.Atan2 ( v_diff.x, v_diff.z );
            me.rotation = Quaternion.Euler(0f, atan2 * Mathf.Rad2Deg,0f );
            
            
            v_diff = (lookTarget - myFace.position);	
            atan2 = Mathf.Atan2 ( v_diff.y, new Vector2(v_diff.x, v_diff.z).magnitude );
            myFace.localRotation = Quaternion.Euler(-atan2 * Mathf.Rad2Deg ,0f, 0f );
            
        }
    }
}

public class AIKnockback : AIBehaviour
{
    private Vector3 source, momentum;
    private float force;

    private float maxKnockbackTime = 1, currentKnockbackTime;
    
    public AIKnockback(Transform transformSource, float strength)
    {
        busy = true;
        source = transformSource.position;
        force = strength;
        
        
    }

    public override void EnterBehaviour(Transform newMe, Transform newFace, Transform target)
    {
        base.EnterBehaviour(newMe, newFace, target);

        Vector3 offset = me.position - source;
        offset.y = 0;
        offset = offset.normalized * force;

        offset.y = 3;

        momentum = offset;
        currentKnockbackTime = maxKnockbackTime;
    }

    public override void Update()
    {
        
        var distToMove = momentum *(currentKnockbackTime/maxKnockbackTime)* Time.deltaTime;
            
        me.position += (Vector3) distToMove;

        currentKnockbackTime -= Time.deltaTime;

        if (currentKnockbackTime <= 0)
        {
            busy = false;
        }
            


    }
}
public class AIIdle : AIBehaviour
{
    
    public override void Update()
    {
    }
}

public class AISearching: AIBehaviour
{
    private float spinSpeed = 90;
    public override void Update()
    {
        myFace.localRotation = new Quaternion();
        me.eulerAngles += new Vector3(0,spinSpeed * Time.deltaTime,0) ;
    }
}

public class AIApproach : AIBehaviour
{
    private float speed;
    private bool moveBackwards;
    private bool useFirstPosition;
    private Vector3 nextPosition;

    private Vector3 myLastPos;

    public AIApproach( float newSpeed, bool movingBackwards = false, bool useFirstPos = false)
    {
        speed = newSpeed;
        moveBackwards = movingBackwards;
        useFirstPosition = useFirstPos;

    }

    public override void EnterBehaviour(Transform newMe, Transform newFace, Transform target)
    {
        base.EnterBehaviour(newMe, newFace, target);
        
        
        nextPosition = myTarget.position;

        myLastPos = me.position;
    }


    public override void Update()
    {
        //Debug.Log("want to move");
        //check if not a wall in the way of approaching
        
        //apply a movement distance on whatever this script is attached to
        if (me != null)
        {
            if (!useFirstPosition)
            {
                nextPosition = myTarget.position;
            }
            rotateToLookAt(nextPosition);
            var distToMove = GetTargetDirection() * speed * Time.deltaTime;
            
            me.position += (Vector3) distToMove;

            //Debug.Log("moved by "+(myLastPos - me.position).magnitude);
            //Debug.Log("compared to "+0.3f*speed* Time.deltaTime);
            if ((myLastPos - me.position).magnitude < 0.5f*speed* Time.deltaTime)
            {
                isStuck = true;
            }
            else
            {
                isStuck = false;
            }

            myLastPos = me.position;
            //Debug.Log("moving by "+ distToMove);
        }
    }
    public Vector3 GetTargetDirection()
    {
        Vector3 newDir =  nextPosition - me.position;

        newDir.y = 0;

        if (newDir.magnitude > 0.1f)
        {
            
            newDir = newDir.normalized;
        }
        else
        {
            newDir = new Vector3();
            busy = false;
        }
            
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

    public AIAttackMelee(float attackCooldown = 1.5f)
    {
        attackCooldownMaxT = attackCooldown;
    }
    
    public override void EnterBehaviour(Transform newMe, Transform newFace, Transform newTarget)
    {
        base.EnterBehaviour(newMe, newFace, newTarget);
        
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
        
        if (attackCooldownCounter > attackCooldownMaxT* 0.2f)
        {
            
            rotateToLookAt(myTarget.position);
            busy = true;
        }

        if (attackCooldownCounter <= 0)
        {
            attackCooldownCounter = attackCooldownMaxT;
            thisMeleeAttacker.Attack();
            busy = false;

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
            
            rotateToLookAt(myTarget.position);
        }

        if (projectileCooldownCounter <= 0)
        {
            projectileCooldownCounter = projectileCooldownMaxT;
            ProjectileManager.Instance.MakeProjectileAt(me.gameObject, myFace, 0); 
        }
        
    }
}

public class AIPatrol : AIBehaviour
{
    public override void Update()
    {
        
    }
}