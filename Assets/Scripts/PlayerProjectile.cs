using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private int projectileIndex;
    // Sets the maximum cooldown for the projectile in the editor
    [SerializeField] private float projectileCooldownCounter,projectileCooldownMax ;
    public ValueGrabber rechargeBarUI;
    
    [SerializeField] private int kunaiAmmo, maxKunaiAmmo;
    public ValueGrabber ammoBarUI;

    [SerializeField] Transform shootTransform;
    
    private void Start()
    {
        
        rechargeBarUI.SetInputMinMax(projectileCooldownMax, 0);
        ammoBarUI.SetInputMinMax(0, maxKunaiAmmo);
        
        
        ChangeAmmoBy(0);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameStateManager.Instance.isPaused)
        {
            return;
        }

        // Decrements the cooldown for the projectile
        // This could possibly be altered by upgrades / power-ups
        if (projectileCooldownCounter > 0)
        {
            projectileCooldownCounter -= Time.deltaTime;
            rechargeBarUI.SetValue(projectileCooldownCounter);
        }
        
        // For the player
        if (Input.GetMouseButtonDown(1) && projectileCooldownCounter <= 0 && kunaiAmmo >= 1)
        {
            ChangeAmmoBy(-1);
            projectileCooldownCounter = projectileCooldownMax;
            ProjectileManager.Instance.MakeProjectileAt(gameObject,shootTransform, projectileIndex);
        }


    }

    public void ChangeAmmoBy(int addAmmo)
    {
        kunaiAmmo += addAmmo;
        kunaiAmmo = Mathf.Clamp(kunaiAmmo, 0, maxKunaiAmmo);
        
        ammoBarUI.SetValue(kunaiAmmo);
    }

    
}
