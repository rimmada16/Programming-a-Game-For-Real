using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private AIBehaviour currentBehaviour;

    private bool alerted,lastAlerted;
    private bool forgotPlayer;
    private bool gotKnockedBack;
    
    [SerializeField] private float approachSpeed, escapeSpeed;
    [SerializeField] private bool ignoreVerticality;

    private Transform target;

    [SerializeField] private float  targetMemoryMaxRemember,targetCheckerFrequency;
    private float targetMemoryTimer,targetCheckerTimer ;
    [SerializeField] private AISpotter spotter;

    [SerializeField] private Transform lookingObject;

    [SerializeField]
    private float maxMeleeDistance,maxProjectileDistance,minProjectileDistance,preferredDistance;

    
    private int distanceComfortable;

    private float previousLowerThreshold, previousHigherThreshold;
    [SerializeField] private float attackCooldown;

    [SerializeField] private float flinchTime = 1;


    // Start is called before the first frame update
    void Start()
    {
        SetBehaviour(new AIIdle(), doExit:false);
        spotter = GetComponent<AISpotter>();
        forgotPlayer = true;

        try
        {

            GetComponent<HealthUnit>().OnDamage += GetKnockedBack;
        }
        catch
        {
            Debug.LogWarning("enemy had no health unit");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //pause check
        if (GameStateManager.Instance.isPaused)
        {
            return;
            
        }

        if (gotKnockedBack)
        {
            if (currentBehaviour.IsBusy())
            {
                currentBehaviour.Update();
                return;
            }
            else
            {
                gotKnockedBack = false;
                InstantSpotPlayer();
            }
            
        }
        
        
        //how often the raycasts for spotting the player are ran
        if (targetCheckerTimer > 0)
        {
            targetCheckerTimer -= Time.deltaTime;
        }
        else
        {
            //Debug.Log("time to look for a player");
            IsPlayerSeen();
            targetCheckerTimer= targetCheckerFrequency;
        }

        //letting the enemy remember where the player is for a little while before forgetting
        if (targetMemoryTimer > 0)
        {
            targetMemoryTimer -= Time.deltaTime;
        }
        else
        {
            if (!forgotPlayer)
            {
                
                ForgetPlayer();
            }
        }
        
        //check that alert state has changed
        if (alerted != lastAlerted)
        {
            //starts seeing player
            if (alerted )
            {
                forgotPlayer = false;
                CheckBehaviourAtDistance();
                //SetBehaviour(new AIAlerted());
            }
            //stops seeing player
            else
            {
                //approaches last known position    
                SetBehaviour(new AIApproach( approachSpeed, useFirstPos:true));
            }
        }
        //while staying the same
        else
        {
            if (alerted)
            {
                if (currentBehaviour.IsStuck())
                {
                    SetBehaviour(new AIAttackProjectile());
                }
                    
                if (PastThresholds())
                {
                    CheckBehaviourAtDistance();
                }
            }
        }

        currentBehaviour.Update();
        
        lastAlerted = alerted;
    }

    private void ForgetPlayer()
    {
        SetBehaviour(new AISearching());
        forgotPlayer = true;
    }
    private void InstantSpotPlayer()
    {
        
        alerted = true;
        target = spotter.GetFirstTarget();
        
        targetMemoryTimer = targetMemoryMaxRemember;
    }

    private void GetKnockedBack(float damage, float knockback,Transform knockbackSource)
    {
        SetBehaviour(new AIKnockback(knockbackSource,knockback, flinchTime), forced: true);
        gotKnockedBack = true;
    }

    private void IsPlayerSeen()
    {
        //Debug.Log("i am checking");
        Transform targetFound = spotter.TryToSpotTargets();


        if (targetFound != null)
        {
            
            alerted = true;
            target = targetFound;
            targetMemoryTimer = targetMemoryMaxRemember;
        }
        else
        {
            
            alerted = false;
        }
    }

    private bool PastThresholds()
    {
        float targetDistance = GetDistanceTo(target);

        if (targetDistance < previousLowerThreshold)
        {
            return true;
        }

        if (targetDistance > previousHigherThreshold)
        {
            return true;
        }
        
        return false;

    }
    
    //decide which behaviour to use at given distance
    private void CheckBehaviourAtDistance()
    {
        float targetDistance = GetDistanceTo(target);
        //Debug.Log(targetDistance+ " distance from player");
        
        //if so close that melee range
        if (targetDistance <= maxMeleeDistance)
        {
            //set behaviour to melee
            SetBehaviour( new AIAttackMelee( attackCooldown ),0, maxMeleeDistance);
            
            return;
        }

        //if too close for projectiles
        if (targetDistance < minProjectileDistance)
        {
            distanceComfortable = -1;//too close
            //set behaviour to approach backwards
            SetBehaviour( new AIApproach(-escapeSpeed,true),maxMeleeDistance,preferredDistance);

            return;
        }
        
        //if too far for projectiles
        if (targetDistance > maxProjectileDistance)
        {
            distanceComfortable = 1;//too far
            //set behaviour to approach forwards
            
            SetBehaviour( new AIApproach(approachSpeed),preferredDistance,9999999);

            return;
        }

        //Debug.Log(distanceComfortable +" comfort level and "+ targetDistance);
        //if wants to move back and has reached comfort distance
        if (distanceComfortable == -1 && targetDistance >= preferredDistance)
        {
            distanceComfortable = 0;
            
        }
        
        //if wants to move forwards and has reached comfort distance
        if (distanceComfortable == 1 && targetDistance <= preferredDistance)
        {
            distanceComfortable = 0;
            
        }
        
        //only if comfortable with given distance
        if (distanceComfortable == 0)
        {
            //set behaviour to shoot projectiles
            SetBehaviour( new AIAttackProjectile(),minProjectileDistance,maxProjectileDistance);
            
            return;
        }
        
    }

    private float GetDistanceTo(Transform other)
    {
        
        var offset = transform.position - other.position;
        if (ignoreVerticality)
        {
            offset.y = 0;
        }
        float distance = offset.magnitude;
        return distance;
    }

    private bool SetBehaviour(AIBehaviour newBehaviour, float prevLow = -1, float prevHigh = -1, bool doExit = true, bool forced = false)
    {
        if (currentBehaviour != null)
        {
            if (currentBehaviour.IsBusy() && !forced )
            {
                return false;
            }
            
            if (doExit)
            {
                currentBehaviour.ExitBehaviour();
            }
        }
        

        if (prevLow >=0)
        {
            previousLowerThreshold = prevLow;
        }
        if (prevHigh >=0)
        {
            previousHigherThreshold = prevHigh;
        }
        
        currentBehaviour = newBehaviour;
        currentBehaviour.EnterBehaviour(transform, lookingObject, target);

        return true;
    }
}
