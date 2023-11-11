using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private int projectileIndex;
    // Sets the maximum cooldown for the projectile in the editor
    public float projectileCooldownMax;
    private float projectileCooldownCounter;
    public int kunaiAmmo;
    [SerializeField] private int maxKunaiAmmo;

    [SerializeField] Transform shootTransform;

    // Update is called once per frame
    void Update()
    {
        // Decrements the cooldown for the projectile
        // This could possibly be altered by upgrades / power-ups
        if (projectileCooldownCounter > 0)
        {
            projectileCooldownCounter -= Time.deltaTime;
        }
        
        // For the player
        if (Input.GetMouseButtonDown(1) && projectileCooldownCounter <= 0 && kunaiAmmo >= 1)
        {
            kunaiAmmo--;
            projectileCooldownCounter = projectileCooldownMax;
            ProjectileManager.Instance.MakeProjectileAt(gameObject,shootTransform, projectileIndex);
        }

        if (kunaiAmmo > maxKunaiAmmo)
        {
            kunaiAmmo--;
        }

    }

    
}
