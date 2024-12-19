using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] InputAction Look;
    // [SerializeField] InputAction LookY;
    [Header("FPS Settings")]
    [SerializeField] GameObject player;
    // [SerializeField] GameObject playerBody;
    [SerializeField] float MouseSensitivity;
    // Start is called before the first frame update
    void Start()
    {
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

    // ReSharper disable Unity.PerformanceAnalysis
    private void ProcessLook()
    {
        Vector2 mouseInput = Look.ReadValue<Vector2>();
        float moveY = mouseInput.x * Time.fixedDeltaTime * MouseSensitivity;
        player.transform.Rotate(Vector3.up * moveY);
        // playerBody.transform.Rotate(Vector3.up * moveY);
    }
}
