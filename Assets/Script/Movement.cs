using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Bindings")]
    [SerializeField] InputAction moveVertically;
    [SerializeField] InputAction moveHorizontally;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void OnEnable()
    {
        moveVertically.Enable();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
