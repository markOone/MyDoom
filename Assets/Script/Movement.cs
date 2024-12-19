using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Bindings")] 
    [SerializeField] InputAction playerMovement;
    
    [Header("References")]
    [SerializeField] Transform playerBody;
    [SerializeField] CharacterController characterController;

    [Header("Movement Settings")] 
    [SerializeField] float movementSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void OnEnable()
    {
        playerMovement.Enable();
    }

    private void OnDisable()
    {
        playerMovement.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        ProcessMovement();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void ProcessMovement()
    {
        Vector2 movement = playerMovement.ReadValue<Vector2>();
        Vector3 movementDirection = transform.forward * movement.y + transform.right * movement.x;
        characterController.Move(movementDirection * movementSpeed * Time.fixedDeltaTime);


    }
}