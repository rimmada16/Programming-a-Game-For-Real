using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUnit : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 10, currentHealth;
    [SerializeField] 
    private float knockbackMultiplier;
    
    [SerializeField]
    private float iFrameTime;
    [SerializeField]
    private float iFrameCounter;

    [SerializeField]
    private bool invincible;

    
    //value grabber link?
    public ValueGrabber healthBarUI;
    
    
    public delegate void GeneralHandler();
    public event GeneralHandler OnHit;
    public event GeneralHandler OnHeal;
    public event GeneralHandler OnDeath;
    
    public delegate void PositionHandler(Vector3 pos);
    public event PositionHandler OnHitAt;
    
    public delegate void DamageHandler(float damage, float knockback, Transform knockbackSource);
    public event DamageHandler OnDamage;
    public Rigidbody myRB;
    private Transform targetTransform;

    // Start is called before the first frame update
    void Start()
    {

        if (healthBarUI != null)
        {
            healthBarUI.SetInputMinMax(0, maxHealth);
        }
        HealthMax();
        myRB = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameStateManager.Instance.isPaused)
        {
            return;
        }
        
        if (iFrameCounter > 0)
        {
            iFrameCounter -= Time.deltaTime;
            //indicator of iframes while active? ie white flashing
        }
    }

    //take damage and return true if the damage got taken
    public bool TakeDamage(int damage, float knockback = 0, Transform knockbackSource = null, Vector3 exactHitPos = new Vector3())
    {
        if (gameObject.CompareTag("Player") && GameStateManager.isHardcore)
        {
            damage = 1000;
        }
        
        
        if (iFrameCounter > 0)
        {
            //Debug.Log("still has iframes");
            return false; //break out if iframes are still up
        }

        if (damage <= 0)
        {
            //Debug.Log("no damage assigned");
            return false;
        }
        
        int newHealth = currentHealth - damage;

        newHealth = Mathf.Clamp(newHealth, 0, maxHealth);//clamp health
        iFrameCounter = iFrameTime;//set iframes
        
        currentHealth = newHealth;
        UpdateDisplay();

        knockback *= knockbackMultiplier;
        CallDamage(damage, knockback, knockbackSource);
        
        CallHit();

        if (exactHitPos == Vector3.zero)
        {
            exactHitPos = transform.position;
        }
        CallHitAt(exactHitPos);
        
        //if unit dies from the new value
        if (newHealth <= 0)
        {
            targetTransform = transform;
            //AudioManager.Instance.PlaySoundAtPoint(1, targetTransform);
            
            CallDeath();
        }
        
        
        if (gameObject.layer == LayerMask.NameToLayer("Prop"))
        {
            DoKnockbackPhysics(knockbackSource,knockback);
        }
        
        
        //run damage effect to indicate taken damage
        return true;
    }

    public void DoKnockbackPhysics(Transform knockbackSource, float knockback)
    {
        if (knockbackSource != null && GetComponent<Rigidbody>())
        {
            
            Vector3 A = transform.position;
            Vector3 B = knockbackSource.position;
            Vector3 forceToApply = A-B;
            forceToApply = forceToApply.normalized;
            forceToApply *= knockback;
            
            myRB.AddForce(forceToApply,ForceMode.Impulse);
        }
    }
    
    //get healed
    public void GetHealed(int heal)
    {
        if (heal <= 0)
        {
            return;
        }
        
        int newHealth = currentHealth + heal;

        newHealth = Mathf.Clamp(newHealth, 0, maxHealth);//clamp health
        currentHealth = newHealth;
        
        CallHeal();
        UpdateDisplay();
        //run healing effects to indicate player got healed
    }

    //instant max heal
    private void HealthMax()
    {
        currentHealth = maxHealth;
        
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (healthBarUI != null)
        {
            healthBarUI.SetValue(currentHealth);
        }
        
    }

    private void CallHit()
    {
        OnHit?.Invoke();
    }

    private void CallHitAt(Vector3 hitPos)
    {
        
        OnHitAt?.Invoke(hitPos);
    }
    private void CallDeath()
    {
        OnDeath?.Invoke();
    }
    private void CallHeal()
    {
        OnHeal?.Invoke();
    }
    private void CallDamage(float damage, float knockback, Transform knockbackSource)
    {
        OnDamage?.Invoke(damage,knockback, knockbackSource);
    }
}
