using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// CURRENTLY CAN ONLY DASH FORWARD
// WALLS MUST BE GIVEN THE "Wall" TAG

// ---Movement while dashing tests---
// Distance reference are the mini squares inside of one of the planes (floor) squares -- grid
// 10 mini squares in one of the plane squares
// When dashing on the ground or in air and not holding a directional key from the right edge of a plane square
// the player travels 9 mini squares
// When dashing on the ground or in the air and holding the backwards directional key from the right edge of the plane
// square the player travels 8 mini squares before travelling backwards

// In my opinion I think letting the player have the ability to micro adjust their dash is something that we should keep

public class BasicDash : MonoBehaviour
{
    private PlayerController _moveScript;
    public float dashSpeed;
    public float dashCooldownMax = 3;
    public float dashCooldownCounter;

    public float dashTimeMax = 1f;
    public float dashTimeCounter = 1f;

    public float range = 2f;

    public ValueGrabber rechargeBarUI;
    
    public delegate void GeneralHandler();
    public event GeneralHandler OnDash;
    

    // Anti clipping measure
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            dashTimeCounter = 0;
            transform.Translate(Vector3.zero);
            Debug.Log("The collision detector did its thing");
        }
    }

    public void EnableSelf(bool nowEnable)
    {
        if (!this.enabled == nowEnable)
        {
            GameStateManager.Instance.dashUi.SetActive(nowEnable);
            this.enabled = nowEnable; 
        }

    }

    // Grabs the player controller at the start
    void Start()
    {
        _moveScript = GetComponent<PlayerController>();
        rechargeBarUI.SetInputMinMax(dashCooldownMax, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance.isPaused)
        {
            return;
        }

        // Slowing decrementing the dash cooldown timer
        if (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
            rechargeBarUI.SetValue(dashCooldownCounter);

        }

        // Raycasting shenanigans - Sets up the Raycast plus a Debug Raycast for use in the editor
        Vector3 direction = Vector3.forward;

        // Define the cast positions and counts
        Vector3[] castPositions = { Vector3.up, Vector3.down };
        int castCount = 20;

        foreach (Vector3 castPosition in castPositions)
        {
            for (int i = 0; i < castCount; i++)
            {
                float castOffset = 0.99f - 0.1f * i;
                
                Vector3 raycastFrom = transform.position + castPosition * castOffset;
                Ray theRaycast = new Ray(raycastFrom, transform.TransformDirection(direction * range));
                Debug.DrawRay(raycastFrom, transform.TransformDirection(direction * range), Color.red);
                
                if (Physics.Raycast(theRaycast, out RaycastHit hit, range) && hit.collider.CompareTag("Wall"))
                {
                    dashTimeCounter = 0;
                    transform.Translate(Vector3.zero);
                    //Debug.Log("The raycast has hit a wall and a dash cannot occur");

                    _moveScript.lockMovement = false;
                }

                // Raycast miss condition - runs the dash code
                else
                {
                    // If the player presses SHIFT :) and the Dash Cooldown is less than or equal to 0 then the Dash
                    // Coroutine will execute
                    if (Input.GetKey(KeyCode.LeftShift) && dashCooldownCounter <= 0)
                    {
                        // Sets the dash to a cooldown of 3s in the script, currently 1s in the editor
                        dashCooldownCounter = dashCooldownMax;

                        _moveScript.lockMovement = true;
                        
                        OnDash?.Invoke();
                        StartCoroutine(Dash());
                    }
                }
            }
        }
        
        // I used a Coroutine for the while loop I think when I made this
        // I did try changing this to be rigidbody based but I couldn't get it to work sadly
        // Also tried pulling it out of the coroutine, while loop did the funny, changed the while
        // then the Dash moved the player like 1mm
        
        // Dash stuff
        IEnumerator Dash()
        {
            float startTime = Time.time;
            // Seems to prevent clipping by having dashTime set to 1 second >>> 12/10/23
            dashTimeCounter = dashTimeMax;
            while (Time.time < startTime + dashTimeCounter && !GameStateManager.Instance.isPaused)
            {
                transform.Translate(Vector3.forward * (dashSpeed * Time.deltaTime));
                yield return null;
            }
            if (Time.time >= startTime + dashTimeCounter)
            {
                _moveScript.lockMovement = false;
            }
        }
    }
}
