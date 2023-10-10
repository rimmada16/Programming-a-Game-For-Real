using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CURRENTLY CAN ONLY DASH FORWARD

// Script for a basic dash - the current iteration allows the player to dash roughly ~7.5m on the ground and in the air
// The script makes use of Raycasting to see if the player is within 5m of a wall, the reasoning for this is to prevent
// clipping, this has been a prevalent issue and a 5m raycast seems to be the lowest I can get it without any clipping occuring
// 1.5m was tried as well as 3m but clipping through walls would occur. The clipping happens due to the way the dash
// is executed via transform.Translate, if rigidbodies were to be made use of in this and the player controller this could perhaps
// be circumvented. Rigidbody would also be less taxing on a computer but that is for another day I suppose.

// Just got the cooldown to work, it is currently set to 3 seconds and there is a console log everytime you press the dash key when
// it is on cooldown

public class BasicDash : MonoBehaviour
{
    private PlayerController _moveScript;
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;
    
    public float range = 5;

    // Start is called before the first frame update
    void Start()
    {
        _moveScript = GetComponent<PlayerController>();
    }
    
    // Update is called once per frame
    void Update()
        {
            // Slowing decrementing the dash cooldown timer
            dashCooldown -= Time.deltaTime;
            
            // Outputs how long is left on the Dash cooldown in the console if the player presses "E"
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("There is currently: " + dashCooldown + "seconds left before the Dash can be used again!");
            }
        
            // Raycasting shenanigans
            // Sets up the Raycast plus a Debug Raycast for use in the editor
            // Raycast was only implemented to try prevent clipping
            // I'm sure there is a way to kill the momentum as soon as the raycast condition becomes true but I am
            // not sure how to implement it and my brain is fried from the amount of reading I have been through today
            Vector3 direction = Vector3.forward;
            Ray theRay = new Ray(transform.position, transform.TransformDirection(direction * range));
            Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));

            // Raycast hit condition
            if (Physics.Raycast(theRay, out RaycastHit hit, range))
            {
                Debug.Log("The raycast has hit a wall and a dash cannot occur");
            }
            
            // Raycast miss condition - runs the dash code
            else
            {
                // If the player presses E and the Dash Cooldown is less than or equal to 0 then the Dash
                // Coroutine will execute
                if (Input.GetKey(KeyCode.E) && dashCooldown <= 0)
                {
                    StartCoroutine(Dash());
                }
            }

            // Dash stuff
            IEnumerator Dash()
            {
                float startTime = Time.time;
                while (Time.time < startTime + dashTime)
                {
                    // Using the translate function causes a fair amount of issues such as the clipping through walls
                    // I think we should move to a rigidbody system unless you can find a solution.
                    transform.Translate(Vector3.forward * dashSpeed);
                    
                    // Sets the dash to a cooldown of 3 seconds
                    dashCooldown = 3;

                    yield return null;
                }
            }
        }
}