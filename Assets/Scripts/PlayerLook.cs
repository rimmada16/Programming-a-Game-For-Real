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
        XYrotation.x = transform.eulerAngles.y;
        
        //cursor lock moved to GameStateManager
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameStateManager.Instance.isPaused)
        {
            return;
        }
        
        Vector2 mouseInput = new Vector2
        {

            x = Input.GetAxis("Mouse X"),
            y = Input.GetAxis("Mouse Y")
        };

        XYrotation.y -= mouseInput.y * sensitivities.y;
        XYrotation.x += mouseInput.x * sensitivities.x;

        XYrotation.y = Mathf.Clamp(XYrotation.y, -90f, 90f);

        transform.eulerAngles = new Vector3(0f, XYrotation.x, 0f);
        playerCam.localEulerAngles = new Vector3(XYrotation.y, 0f, 0f);
    }
    
    
}