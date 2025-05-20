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

        // Start is called before the first frame update
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

        // ReSharper disable Unity.PerformanceAnalysis
        // private void ProcessLookFixed()
        // {
        //     Vector2 mouseInput = Look.ReadValue<Vector2>();
        //     float moveY = mouseInput.x * Time.fixedDeltaTime * MouseSensitivity;
        //     player.transform.Rotate(Vector3.up * moveY);
        //     // playerBody.transform.Rotate(Vector3.up * moveY);
        // }

        void ProcessLook()
        {
            Vector2 mouseInput = Look.ReadValue<Vector2>();
            float moveY = mouseInput.x * Time.deltaTime * MouseSensitivity;
            player.transform.Rotate(Vector3.up * moveY);
        }
    }
}