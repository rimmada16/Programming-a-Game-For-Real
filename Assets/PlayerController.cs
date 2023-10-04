using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float moveSmoothTime,gravityStrength,jumpStrength,walkSpeed,runSpeed;

    [SerializeField] 
    private CharacterController controller;
    
    private Vector3 _currentMoveVelocity,_moveDampVelocity;
    private Vector3 _currentForceVelocity;

     
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>(); 
    }

    // Update is called once per frame
    void Update()
    {


        Vector3 playerInput = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical")
        };


        if (playerInput.magnitude > 1)
        {
            playerInput.Normalize();
        }

        Vector3 moveVector = transform.TransformDirection(playerInput);
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        _currentMoveVelocity = Vector3.SmoothDamp(
            _currentMoveVelocity,
            moveVector * currentSpeed,
            ref _moveDampVelocity,
            moveSmoothTime
        );

        controller.Move(_currentMoveVelocity * Time.deltaTime);

        Ray groundCheckRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundCheckRay, 1.1f))
        {
            _currentForceVelocity.y = -2f;

            if (Input.GetKey(KeyCode.Space))
            {
                _currentForceVelocity.y = jumpStrength;
            }
        }
        else
        {
            //Debug.Log("gravitied");
            _currentForceVelocity.y -= gravityStrength * Time.deltaTime;
        }

        controller.Move(_currentForceVelocity * Time.deltaTime);
    }

}