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
    
    
    public delegate void DeathHandler();
    public event DeathHandler OnDeath;
    public Rigidbody myRB;
    
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
    public bool TakeDamage(int damage, float knockback = 0, Transform knockbackSource = null)
    {
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
        
        //if unit dies from the new value
        if (newHealth <= 0)
        {
            CallDeath();
        }

        
        if (knockbackSource != null && GetComponent<Rigidbody>())
        {
            
            Vector3 A = transform.position;
            Vector3 B = knockbackSource.position;
            Vector3 forceToApply = A-B;
            forceToApply = forceToApply.normalized;
            forceToApply *= knockback;
            
            myRB.AddForce(forceToApply,ForceMode.Impulse);
        }
        
        //run damage effect to indicate taken damage
        return true;
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

    private void CallDeath()
    {
        OnDeath?.Invoke();
    }
}
