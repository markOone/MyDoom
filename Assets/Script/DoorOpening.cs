using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorOpening : MonoBehaviour
{
    [SerializeField] InputAction openBind;
    [SerializeField] Transform camera;
    
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
            if (PlayerShooting.Instance.shootingField.DoorsInField.Count > 0)
            {
                foreach (GameObject obj in PlayerShooting.Instance.shootingField.DoorsInField)
                {
                    IOpenable openable = obj.transform.gameObject.GetComponent<IOpenable>();
                    openable?.Open();
                }
            }
        }
    }
}
