using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MeleeAttackTest : MonoBehaviour
{
    [SerializeField] private Collider SlashCollider;

    [SerializeField]
    private float SlashTime = 0.2f, SlashCooldown = 0.4f ,SlashCounter ;

    [SerializeField] private int Damage;

    [SerializeField] private bool ColliderOn;
    
    //test
    [SerializeField] private Transform impactPoint;
    public float tempKnockback;
    
    // Start is called before the first frame update
    void Start()
    {
        SlashCollider.gameObject.GetComponent<ColliderHitbox>().OnCollide += AttackCollided;
        
        SlashTime = Mathf.Clamp(SlashTime, 0, SlashCooldown);
        EndCollision();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance.isPaused)
        {
            return;
        }

        //decrement the counter
        if (SlashCounter > 0)
        {
            SlashCounter -= Time.deltaTime;
        }
        
        //if the cooldown is [SlashTime] down, switch of the collider
        if (SlashCounter <= SlashCooldown - SlashTime && ColliderOn)
        {
            EndCollision();
        }
        
        
        //if the cooldown is entirely down, another slash can run
        if (Input.GetMouseButtonDown(0) && SlashCounter <= 0)
        {
            StartCollision();
        }
    }

    void StartCollision()
    {
        SlashCounter = SlashCooldown;
        SlashCollider.enabled = true;
        ColliderOn = true;
        Debug.Log("ColliderOn "+ SlashCounter);
    }

    void EndCollision()
    {
        
        SlashCollider.enabled = false;
        
        ColliderOn = false;
        Debug.Log("ColliderOff "+ SlashCounter);
    }

    void AttackCollided(Collider otherCol)
    {
        if (otherCol.gameObject.layer == gameObject.layer)
        {
            return;
        }

        if (otherCol.gameObject.layer == LayerMask.GetMask("Terrain"))
        {
            return;
        }
        
        HealthUnit otherHU = otherCol.GetComponent<HealthUnit>();
        if (otherHU == null)
        {
            return;
        }
        //-------------------------
        //temporary knockback
        Rigidbody otherRB = otherCol.gameObject.GetComponent<Rigidbody>();
        Vector3 A = impactPoint.position;
        Vector3 B = otherCol.transform.position;
        Vector3 forceToApply = B-A;
        forceToApply = forceToApply.normalized;
        forceToApply *= tempKnockback;
        
        otherRB.AddForce(forceToApply,ForceMode.Impulse);
        //otherRB.AddForce(0,3,0, ForceMode.Impulse);
        //--------------------------------------
        
        
        otherHU.TakeDamage(Damage);

    }
}
