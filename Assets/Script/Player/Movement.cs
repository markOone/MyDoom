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


        // Update is called once per frame
        void Update()
        {
        }

        private void FixedUpdate()
        {
            ProcessMovement();
            ProccessMovementAnimation();
            velocity.y += Physics.gravity.y * Time.fixedDeltaTime;
            characterController.Move(velocity * Time.fixedDeltaTime);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        void ProcessMovement()
        {
            Vector2 movement = playerMovement.ReadValue<Vector2>();
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