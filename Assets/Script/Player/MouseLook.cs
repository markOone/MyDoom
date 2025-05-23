using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyDoom.Player
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] InputAction Look;

        [Header("FPS Settings")] [SerializeField]
        GameObject player;

        [SerializeField] float MouseSensitivity;
        
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            Look.Enable();
        }

        private void OnDisable()
        {
            Look.Disable();
        }

        void Update()
        {
            ProcessLook();
        }

        void ProcessLook()
        {
            Vector2 mouseInput = Look.ReadValue<Vector2>();
            float moveY = mouseInput.x * Time.deltaTime * MouseSensitivity;
            player.transform.Rotate(Vector3.up * moveY);
        }
    }
}