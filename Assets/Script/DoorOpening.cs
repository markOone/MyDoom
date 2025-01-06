using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorOpening : MonoBehaviour
{
    [SerializeField] InputAction openBind;
    [SerializeField] Camera CameraTransform;
    
    private void OnEnable()
    {
        openBind.Enable();
    }
    
    private void OnDisable()
    {
        openBind.Disable();
    }

    private void FixedUpdate()
    {
        ProccessDoor();
    }
    
    private void ProccessDoor()
    {
        if (openBind.IsPressed())
        {
            Debug.Log("Pressed");
            if (Physics.Raycast(CameraTransform.transform.position, CameraTransform.transform.forward, out RaycastHit hit, 10f))
            {
                IOpenable openable = hit.transform.gameObject.GetComponent<IOpenable>();
                openable?.Open();
            }
        }
    }
}
