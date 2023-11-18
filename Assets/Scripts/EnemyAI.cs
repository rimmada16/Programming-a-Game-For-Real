using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private AIBehaviour currentBehaviour;

    
    [SerializeField]
    private bool alerted,lastAlerted;

    [SerializeField] private float approachSpeed, escapeSpeed;
    [SerializeField] private bool ignoreVerticality;

    [SerializeField]
    private Transform target;

    [SerializeField] private float targetMemoryTimer, targetMemoryMaxRemember;
    [SerializeField] private float targetCheckerTimer, targetCheckerFrequency;
    [SerializeField] private AISpotter spotter;

    [SerializeField] private Transform lookingObject;

    [SerializeField]
    private float maxMeleeDistance,maxProjectileDistance,minProjectileDistance,preferredDistance;

    [SerializeField] private int distanceComfortable;

    [SerializeField] private float previousLowerThreshold, previousHigherThreshold;


    // Start is called before the first frame update
    void Start()
    {
        SetBehaviour(new AIIdle(), false);
        spotter = GetComponent<AISpotter>();

        try
        {

            GetComponent<HealthUnit>().OnDamage += InstantSpotPlayer;
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
            ForgetPlayer();
        }
        
        //check that alert state has changed
        if (alerted != lastAlerted)
        {
            //changed to be alert
            if (alerted )
            {
                CheckBehaviourAtDistance();
                //SetBehaviour(new AIAlerted());
            }
            //changed to not be alert
            else
            {
                SetBehaviour(new AIIdle());
            }
        }
        //while staying the same
        else
        {
            if (alerted)
            {
                if (PastThresholds())
                {
                    CheckBehaviourAtDistance();
                }
            }
        }

        currentBehaviour.Update();
        
        lastAlerted = alerted;
    }

    private void InstantSpotPlayer()
    {
        
        alerted = true;
        target = spotter.GetFirstTarget();
        targetMemoryTimer = targetMemoryMaxRemember;
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
    }

    private void ForgetPlayer()
    {
        alerted = false;
        target = null;
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
            SetBehaviour( new AIAttackMelee());

            
            previousLowerThreshold = 0;
            previousHigherThreshold = maxMeleeDistance;
            return;
        }

        //if too close for projectiles
        if (targetDistance < minProjectileDistance)
        {
            distanceComfortable = -1;//too close
            //set behaviour to approach backwards
            SetBehaviour( new AIApproach(target,-escapeSpeed,true));

            previousLowerThreshold = maxMeleeDistance;
            previousHigherThreshold = preferredDistance;
            return;
        }
        
        //if too far for projectiles
        if (targetDistance > maxProjectileDistance)
        {
            distanceComfortable = 1;//too far
            //set behaviour to approach forwards
            
            SetBehaviour( new AIApproach(target,approachSpeed));

            previousLowerThreshold = preferredDistance;
            previousHigherThreshold = 9999999;
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
            SetBehaviour( new AIAttackProjectile());
            
            
            previousLowerThreshold = minProjectileDistance;
            previousHigherThreshold = maxProjectileDistance;
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

    private void SetBehaviour(AIBehaviour newBehaviour, bool doExit = true)
    {
        if (doExit)
        {
            currentBehaviour.ExitBehaviour();
        }
        currentBehaviour = newBehaviour;
        currentBehaviour.EnterBehaviour(transform, lookingObject, target);
    }
}
