using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField]
    private Transform playerCam;
    [SerializeField]
    private Vector2 sensitivities;

    private Vector2 XYrotation;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseInput = new Vector2
        {

            x = Input.GetAxis("Mouse X"),
            y = Input.GetAxis("Mouse Y")
        };

        XYrotation.x -= mouseInput.y * sensitivities.y;
        XYrotation.y += mouseInput.x * sensitivities.x;

        XYrotation.x = Mathf.Clamp(XYrotation.x, -90f, 90f);

        transform.eulerAngles = new Vector3(0f, XYrotation.y, 0f);
        playerCam.localEulerAngles = new Vector3(XYrotation.x, 0f, 0f);
    }
    
    
}