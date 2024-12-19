using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] InputAction Look;
    // [SerializeField] InputAction LookY;
    [Header("FPS Settings")]
    Camera FPSCamera;
    [SerializeField] float MouseSensitivity;
    // Start is called before the first frame update
    void Start()
    {
        FPSCamera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnEnable()
    {
        Look.Enable();
    }
    
    private void OnDisable()
    {
        Look.Disable();
    }

    void FixedUpdate()
    {
        ProcessLook();
    }

    private void ProcessLook()
    {
        Vector2 mouseInput = Look.ReadValue<Vector2>();
        float moveY = mouseInput.x * Time.fixedDeltaTime * MouseSensitivity;
        FPSCamera.gameObject.transform.Rotate(Vector3.up * moveY);
    }
}
