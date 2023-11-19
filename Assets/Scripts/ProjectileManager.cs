using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    [SerializeField] private
        GameObject[] projectilePrefabs;
    
    public void MakeProjectileAt(GameObject source, Transform sourceTransform = null, int projectileIndex =0)
    {
        if (sourceTransform == null)
        {
            sourceTransform = source.transform;
        }
        // Spawns the projectile 2m in front of the player / enemy
        var newProjectile = Instantiate(projectilePrefabs[projectileIndex]);
        newProjectile.layer = gameObject.layer;
        newProjectile.GetComponent<ProjectileMovement>().source = source.transform;
        
        var newTransform = newProjectile.transform;
        newTransform.position = sourceTransform.position;
        newTransform.rotation = sourceTransform.rotation;
        newTransform.parent = transform;

        newProjectile.layer = source.layer;

        
        //Debug.Log("The projectile has been created");
        
    }
}