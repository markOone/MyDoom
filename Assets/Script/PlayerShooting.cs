using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    private static PlayerShooting _instance;
        
    [Header("Weapon Properties")] 
    [SerializeField] internal GunData currentGunData;
    [SerializeField] internal Gun currentGun;
    
    [Header("Bindings")]
    [SerializeField] InputAction shootingBind;
    
    
    [Header("References")]
    [SerializeField] Animator weaponAnimator;
    [SerializeField] Animator flashlightAnimator;
    PlayerStats playerStats;
    [SerializeField] internal GameObject metalImpactEffect;
    [SerializeField] internal GameObject stoneImpactEffect;
    [SerializeField] internal GameObject enemyImpactEffect;

    public static PlayerShooting Instance { get { if (_instance == null) Debug.Log("No GameManager"); return _instance; } }

    private void OnEnable()
    {
        shootingBind.Enable();
    }

    private void OnDisable()
    {
        shootingBind.Disable();
    }
    
    private void Awake()
    {
        _instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerStats.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        ProccessShooting();
    }

    public void ProccessShooting()
    {
        if (shootingBind.IsPressed() && currentGunData.currentAmmo > 0)
        {
            currentGun.Shoot();
        }
    }

    public void Shoot2D()
    {
            weaponAnimator.SetTrigger("Shooting");
            flashlightAnimator.SetTrigger("Shooting");
    }
}
