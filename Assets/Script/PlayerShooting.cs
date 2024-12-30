using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Weapon Properties")] 
    bool canShoot = true;
    float weaponCountDown = 1.3f;
    [SerializeField] List<string> guns = new List<string>();
    private int currentGun = 1;
    
    [Header("Bindings")]
    [SerializeField] InputAction shootingBind;
    
    [Header("References")]
    [SerializeField] ShootingField shootingField;
    [SerializeField] Animator weaponAnimator;
    [SerializeField] Animator flashlightAnimator;
    PlayerStats playerStats;
    
    private void OnEnable()
    {
        shootingBind.Enable();
    }

    private void OnDisable()
    {
        shootingBind.Disable();
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
        if (shootingBind.IsPressed())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (PlayerStats.Instance.ShellCounter > 0 && canShoot)
        {
            StartCoroutine(ShootingCountDown());
            weaponAnimator.SetTrigger("Shooting");
            flashlightAnimator.SetTrigger("Shooting");
            
            if (shootingField.EnemiesInField.Count > 0)
            {
                foreach (var enemy in shootingField.EnemiesInField)
                {
                    //enemy.GetComponent<EnemyScript>().TakeDamage(damage);
                }
            }

            if (currentGun == 0) PlayerStats.Instance.BulletsCounter -= 1;
            if (currentGun == 1) PlayerStats.Instance.ShellCounter -= 1;
            if (currentGun == 2) PlayerStats.Instance.RocketsCounter -= 1;
            
            HudController.Instance.UpdateHUD();
        }
    }

    IEnumerator ShootingCountDown()
    {
        canShoot = false;
        yield return new WaitForSeconds(weaponCountDown);
        canShoot = true;
    }
}
