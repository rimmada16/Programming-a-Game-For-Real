using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CURRENTLY CAN ONLY DASH FORWARD
// WALLS MUST BE GIVEN THE "Wall" TAG

// -- PRE ANTI CLIPPING MEASURES - Left these in for context --
// Script for a basic dash - the current iteration allows the player to dash roughly ~5m on the ground and in the air
// The script makes use of Raycasting to see if the player is within 5m of a wall, the reasoning for this is to prevent
// clipping, this has been a prevalent issue and a 5m raycast seems to be the lowest I can get it without any clipping occuring
// 1.5m was tried as well as 3m but clipping through walls would occur. The clipping happens due to the way the dash
// is executed via transform.Translate, if rigidbodies were to be made use of in this and the player controller this could perhaps
// be circumvented. Rigidbody would also be less taxing on a computer but that is for another day I suppose.

// -- 12/10/23 -- (Adam)
// Cooldown now works - Set to 3 seconds.
// Dash speed now set to 0.3 - Anything higher than 0.5 seems to be too fast.
// Made it so the dashTime is non adjustable in the editor and set to a constant of 1s, this seems to prevent clipping.
// Added another method of preventing clipping using collision detection with tags.
// Ground and Jump dash now move the player ~15m.
// Raycast set to 1.5m


// Some additions I would like to make are so that you keep all momentum when exiting a dash and make it so the player
// can dash in any direction.

public class BasicDash : MonoBehaviour
{
    private PlayerController _moveScript;
    public float dashSpeed;
    public float dashCooldownMax = 3;
    public float dashCooldownCounter;
    private float dashTime = 1f;
    
    public float range = 2f;

    public valueGrabber rechargeBarUI;

    // Another attempt at preventing clipping
    // If the object has the tag of wall then it will set the dashTime to 0 so that it ends and will translate the
    // gameobject by nothing -- seems to work
    // Had to add a Rigidbody to the player for this to work
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Wall")
        {
            dashTime = 0;
            transform.Translate(new Vector3(0, 0, 0 ));
            Debug.Log("The collision detector did its thing");
        }
    }
    
    // Grabs the player controller at the start
    void Start()
    {
        _moveScript = GetComponent<PlayerController>();
    }
    
    // Update is called once per frame
    void Update()
        {
            // Slowing decrementing the dash cooldown timer
            if (dashCooldownCounter > 0)
            {
                dashCooldownCounter-= Time.deltaTime;
                rechargeBarUI.SetValue(dashCooldownCounter);
            }
            
            
            // Outputs how long is left on the Dash cooldown in the console if the player presses "E"
            // Was quite spammy so commented it out, enable at own peril
            if (Input.GetKey(KeyCode.E))
            {
                // Debug.Log("There is currently: " + dashCooldown + "seconds left before the Dash can be used again!");
            }
        
            // Raycasting shenanigans
            // Sets up the Raycast plus a Debug Raycast for use in the editor
            // Used to prevent clipping
            Vector3 direction = Vector3.forward;
            // direction * range should be 1 * 1.5
            Ray theRay = new Ray(transform.position, transform.TransformDirection(direction * range));
            Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));

            // Raycast hit condition
            if (Physics.Raycast(theRay, out RaycastHit hit, range))
            {
                if (hit.collider.tag == "Wall")
                {
                    dashTime = 0;
                    transform.Translate(new Vector3(0, 0, 0 ));
                    Debug.Log("The raycast has hit a wall and a dash cannot occur");
                    // Very spammy, enable at will   
                }
            }
            
            // Raycast miss condition - runs the dash code
            else
            {
                // If the player presses E and the Dash Cooldown is less than or equal to 0 then the Dash
                // Coroutine will execute
                if (Input.GetKey(KeyCode.E) && dashCooldownCounter <= 0)
                {
                    StartCoroutine(Dash());
                }
            }
            
            // Dash stuff
            IEnumerator Dash()
            {
                float startTime = Time.time;
                // Seems to prevent clipping by having dashTime set to 1 second >>> 12/10/23
                dashTime = 1f;
                while (Time.time < startTime + dashTime)
                {
                    transform.Translate(Vector3.forward * dashSpeed);
                    
                    // Sets the dash to a cooldown of 3 seconds
                    dashCooldownCounter = dashCooldownMax;

                    yield return null;
                }
            }
        }
}