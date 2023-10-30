using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUnit : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 10, currentHealth;
    
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
    
    // Start is called before the first frame update
    void Start()
    {

        if (healthBarUI != null)
        {
            healthBarUI.SetInputMinMax(0, maxHealth);
        }
        HealthMax();
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
    public bool TakeDamage(int damage)
    {
        if (iFrameCounter > 0)
        {
            return false; //break out if iframes are still up
        }

        if (damage <= 0)
        {
            return false;
        }
        
        int newHealth = currentHealth - damage;

        newHealth = Mathf.Clamp(newHealth, 0, maxHealth);//clamp health
        iFrameCounter = iFrameTime;//set iframes
        
        currentHealth = newHealth;
        UpdateDisplay();
        
        //if player dies from the new value
        if (newHealth <= 0)
        {
            CallDeath();
        }
        //run damage effect to indicate player took damage
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
