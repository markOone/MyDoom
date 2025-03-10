using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorOpening : MonoBehaviour
{
    [SerializeField] InputAction interactionBind;//interactionBind
    [SerializeField] Camera CameraTransform;
    
    private void OnEnable()
    {
        interactionBind.Enable();
    }
    
    private void OnDisable()
    {
        interactionBind.Disable();
    }

    private void FixedUpdate()
    {
        ProccessDoor();
    }
    
    private void ProccessDoor()
    {
        if (interactionBind.IsPressed())
        {
            Debug.Log("Pressed");
            if (Physics.Raycast(CameraTransform.transform.position, CameraTransform.transform.forward, out RaycastHit hit, 10f))
            {
                IInteractable iinteratable = hit.transform.gameObject.GetComponent<IInteractable>();
                iinteratable?.Interact();
                Debug.DrawRay(CameraTransform.transform.position, CameraTransform.transform.forward * 10f, Color.red);
            }
        }
    }
}
