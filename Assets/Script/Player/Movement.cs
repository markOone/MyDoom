using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

namespace MyDoom.Player
{
    public class Movement : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        
        Vector2 movement;

        [Header("Bindings")] [SerializeField] InputAction playerMovement;

        [Header("References")] [SerializeField]
        Transform playerBody;

        [SerializeField] CharacterController characterController;
        [SerializeField] Animator animator;

        [Header("Movement Settings")] [SerializeField]
        float movementSpeed;

        Vector3 velocity;

        private void OnEnable()
        {
            playerMovement.Enable();
        }

        private void OnDisable()
        {
            playerMovement.Disable();
        }

        private void FixedUpdate()
        {
            ProcessMovement();
            ProccessMovementAnimation();
            velocity.y += Physics.gravity.y * Time.fixedDeltaTime;
            characterController.Move(velocity * Time.fixedDeltaTime);
        }
        
        public void ProcessMovement(InputAction.CallbackContext context)
        {
            movement = context.ReadValue<Vector2>();
        }
        
        void ProcessMovement()
        {
            Vector3 movementDirection = transform.forward * movement.y + transform.right * movement.x;
            characterController.Move(movementDirection * movementSpeed * Time.fixedDeltaTime);
        }

        void ProccessMovementAnimation()
        {
            if (playerMovement.IsPressed() && !animator.GetCurrentAnimatorStateInfo(0).IsName("IsMoving"))
            {
                animator.SetInteger("IsMoving", 1);
            }
            else
            {
                animator.SetInteger("IsMoving", 0);
            }
        }
    }
}