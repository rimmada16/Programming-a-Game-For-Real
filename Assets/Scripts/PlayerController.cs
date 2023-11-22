using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSmoothTime,gravityStrength,jumpStrength,walkSpeed;

    [SerializeField]
    private float altSpeed;

    [SerializeField] 
    private CharacterController controller;
    
    private Vector3 _currentMoveVelocity,_moveDampVelocity;
    private Vector3 _currentForceVelocity;
    

    public bool lockMovement;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        lockMovement = false;
    }

    void Update()
    {
        //pause check
        if (GameStateManager.Instance.isPaused)
        {
            return;
        }
        
        if (lockMovement)
        {
            _currentForceVelocity = Vector3.zero;
            //Debug.Log(_currentForceVelocity);
            return;
        }

        
        //get raw inputs
        Vector3 playerInput = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical")
        };
        
        //normalise if being inputted
        if (playerInput.magnitude > 1)
        {
            playerInput.Normalize();
        }

        

        //get forward
        Vector3 moveVector = transform.TransformDirection(playerInput);
        
        //if left shift being held down, use a different speed value
        float currentSpeed = walkSpeed;

        //dampen the movement velocity
        _currentMoveVelocity = Vector3.SmoothDamp(
            _currentMoveVelocity,
            moveVector * currentSpeed,
            ref _moveDampVelocity,
            moveSmoothTime
        );


        
        //-----------vertical motion-----------
        
        //true if grounded
        bool castHitFloor = controller.isGrounded;
        
        
        if (castHitFloor)
        {
            //provide a constant downward pull even when on the floor

            _currentForceVelocity.y = Mathf.Clamp(_currentForceVelocity.y, -100, -2);
            //_currentForceVelocity.y = -2f;

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

        //------------apply all motions--------------
        
        controller.Move(_currentForceVelocity * Time.deltaTime + _currentMoveVelocity * Time.deltaTime);
    }

}