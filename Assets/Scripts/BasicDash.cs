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

    // Anti clipping measure
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Wall")
        {
            dashTimeCounter = 0;
            transform.Translate(Vector3.zero);
            Debug.Log("The collision detector did its thing");
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
        Ray theRay = new Ray(transform.position, transform.TransformDirection(direction * range));
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));

        // Raycast hit condition
        if (Physics.Raycast(theRay, out RaycastHit hit, range) && hit.collider.tag == "Wall")
        {
            dashTimeCounter = 0;
            transform.Translate(Vector3.zero);
            Debug.Log("The raycast has hit a wall and a dash cannot occur");

            _moveScript.lockMovement = false;
        }

        // Raycast miss condition - runs the dash code
        else
        {
            // If the player presses E and the Dash Cooldown is less than or equal to 0 then the Dash
            // Coroutine will execute
            if (Input.GetKey(KeyCode.E) && dashCooldownCounter <= 0)
            {
                // Sets the dash to a cooldown of 3s in the script, currently 1s in the editor
                dashCooldownCounter = dashCooldownMax;

                _moveScript.lockMovement = true;
                StartCoroutine(Dash());
            }
        }

        // Dash stuff
        IEnumerator Dash()
        {
            float startTime = Time.time;
            // Seems to prevent clipping by having dashTime set to 1 second >>> 12/10/23
            dashTimeCounter = dashTimeMax;
            while (Time.time < startTime + dashTimeCounter && !GameStateManager.Instance.isPaused)
            {

                transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);
                yield return null;
            }
            if (Time.time >= startTime + dashTimeCounter)
            {
                _moveScript.lockMovement = false;
            }

            yield return null;
        }
    }
}
