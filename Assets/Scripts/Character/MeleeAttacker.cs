using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttacker : MonoBehaviour
{
    [SerializeField]
    private bool locked;

    [SerializeField]
    private float cooldownCounter,timeBeforeCanAttack,extraTimeForFullDamage;

    [SerializeField] private Transform castSource, knockbackSource;
    [SerializeField] private int rayAmount;
    [SerializeField] private float rayDistance, rayAngleCoverage, rayAngleCoverageDud;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private int damage, damageDud;
    [SerializeField] private float knockback, knockbackDud;

    [SerializeField] 
    private ValueGrabber cooldownBarUI;

    public delegate void GeneralHandler();
    public event GeneralHandler OnAttackSuccess;
    public event GeneralHandler OnAttackDud;

    private void Start()
    {

        if (cooldownBarUI != null)
        {
            cooldownBarUI.SetInputMinMax(0, timeBeforeCanAttack+ extraTimeForFullDamage);
            cooldownCounter = 0;
            cooldownBarUI.SetValue(cooldownCounter);
        }
    }

    private void Update()
    {
        if (GameStateManager.Instance.isPaused)
        {
            return;
        }

        if (cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
            if (cooldownBarUI != null)
            {
                cooldownBarUI.SetValue(cooldownCounter);
            }
        }
        
    }


    public float Attack() //returns intensity of damage dealt, ie for screenshake
    {
        if (locked || castSource == null)
        {
            return 0;
        }
        
        //----------------------counter----------------------------------
        
        //if timeBeforeCanAttack not passed yet
        if (cooldownCounter > extraTimeForFullDamage)
        {
            return 0;
        }
        
        //values that can vary based on checks
        float effectiveRayAngleCoverage;
        int effectiveDamage;
        float effectiveKnockback;
        
        //true if fully completed both cooldowns
        if (cooldownCounter <= 0) //if player waited for cooldown to be off
        {
            //Debug.Log("attacked fully off cooldown");
            effectiveRayAngleCoverage = rayAngleCoverage;
            effectiveKnockback = knockback;
            effectiveDamage = damage; 
            OnSlash();
            
            
        }
        else //if player attacked while cooldown was still going down
        {
            //Debug.Log("attacked with a dud");
            effectiveRayAngleCoverage = rayAngleCoverageDud;
            effectiveDamage = damageDud;
            effectiveKnockback = knockbackDud;
            OnDud();
        }
        
        //set timer for next time
        cooldownCounter = timeBeforeCanAttack + extraTimeForFullDamage;
        
        //----------------------raycasts----------------------------------

        //origin: castSource.position, direction: castSource.forward 
        RaycastHit[] allResults = new RaycastHit[rayAmount];



        for (int i = 0; i < rayAmount; i++)
        {
            Vector3 newDirection = castSource.forward;
            if (rayAmount > 1)
            {
                float anglePerSegment = (effectiveRayAngleCoverage) /(rayAmount-1) ;
                float angleToRotBy;
                angleToRotBy = i * anglePerSegment;
                angleToRotBy -= effectiveRayAngleCoverage / 2;
                newDirection = Quaternion.AngleAxis(angleToRotBy, castSource.up) * newDirection;
                newDirection = newDirection.normalized;

            }
            
            RaycastHit[] newResults = new RaycastHit[1];
            
            Physics.RaycastNonAlloc(
                origin:castSource.position, 
                direction:newDirection, 
                results: newResults, 
                maxDistance: rayDistance,
                layerMask: layerMask
            );

            if (newResults[0].transform != null)
            {
                allResults[i] = newResults[0];
            }
            
        }
        
        //----------------------apply damage----------------------------------

        //tally damage done for extra effects
        float damageDone = 0;

        foreach (var hit in allResults)
        {
            //skip if failed
            if (hit.transform == null)
            {
                continue;
            }

            //skip if failed
            var hitHU = hit.transform.GetComponent<HealthUnit>();
            if (hitHU == null)
            {
                continue;
            }

            //if a knockback source hasnt been assigned, use itself
            Transform sourceOfKnockback = knockbackSource;
            if (sourceOfKnockback == null)
            {
                sourceOfKnockback = transform;
            }
            
            //deal damage but check if it was stopped by iframes
            if (hitHU.TakeDamage(effectiveDamage, effectiveKnockback, knockbackSource));
            {
                damageDone += effectiveDamage;
            }
            
        }

        return damageDone;
    }

    void OnSlash()
    {
        OnAttackSuccess?.Invoke();
    }
    
    void OnDud()
    {
        
        OnAttackDud?.Invoke();
    }
    //---Gizmos--------------------------------------------------------------
    private void OnDrawGizmos()
    {
        
        if (locked || castSource == null)
        {
            return;
        }

        
        for (int i = 0; i < rayAmount; i++)
        {

            Vector3 newDirection = castSource.forward;
            if (rayAmount > 1)
            {
                float anglePerSegment = (rayAngleCoverage) /(rayAmount-1) ;
                float angleToRotBy;
                angleToRotBy = i * anglePerSegment;
                angleToRotBy -= rayAngleCoverage / 2;
                newDirection = Quaternion.AngleAxis(angleToRotBy, castSource.up) * newDirection;
                newDirection = newDirection.normalized;

            }
            
            Gizmos.DrawLine(castSource.position, castSource.position+(newDirection*rayDistance));
            
        }
        
    }
    
}
