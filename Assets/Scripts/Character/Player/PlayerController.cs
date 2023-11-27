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
    
    public delegate void GeneralHandler();
    public event GeneralHandler OnJump;

    public bool lockMovement;

    [SerializeField] private Vector3 groundSphereCastOrigin;
    [SerializeField] private float groundSphereCastDist;
    [SerializeField] private float groundSphereCastRadius;
    
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
        bool controllerIsGrounded = controller.isGrounded;

        bool castHit =  Physics.SphereCast(origin: groundSphereCastOrigin+ transform.position, radius: groundSphereCastRadius,
            direction: Vector3.down, hitInfo: out _, maxDistance:groundSphereCastDist, layerMask: LayerMask.GetMask("Terrain", "Prop"));
        
        bool castHitHead =  Physics.SphereCast(origin: groundSphereCastOrigin+ transform.position, radius: groundSphereCastRadius,
            direction: Vector3.up, hitInfo: out _, maxDistance:groundSphereCastDist, layerMask: LayerMask.GetMask("Terrain"));

        if (controllerIsGrounded && castHit)
        {
            //provide a constant downward pull even when on the floor

            //_currentForceVelocity.y = Mathf.Clamp(_currentForceVelocity.y, -100, -2);
            //_currentForceVelocity.y = -2f;

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            //Debug.Log("gravitied");
            _currentForceVelocity.y -= gravityStrength * Time.deltaTime;
        }
        
        if (castHitHead)
        {
            //provide a constant downward pull even when on the floor

            _currentForceVelocity.y = Mathf.Clamp(_currentForceVelocity.y, -100, 0);
            //_currentForceVelocity.y = -2f;

        }


        //------------apply all motions--------------
        
        controller.Move(_currentForceVelocity * Time.deltaTime + _currentMoveVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        _currentForceVelocity.y = jumpStrength;
        OnJump?.Invoke();
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundSphereCastOrigin+ transform.position, groundSphereCastRadius );
        Gizmos.DrawWireSphere(groundSphereCastOrigin + Vector3.down*groundSphereCastDist+ transform.position, groundSphereCastRadius );
    }
}