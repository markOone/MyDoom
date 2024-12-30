using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [Header("Weapon Properties")] 
    bool canShoot = true;
    float weaponCountDown = 1.3f;
    
    [Header("Bindings")]
    [SerializeField] InputAction shootingBind;
    
    [Header("References")]
    [SerializeField] ShootingField shootingField;
    [SerializeField] Animator weaponAnimator;
    [SerializeField] Animator flashlightAnimator;
    
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
        }
    }

    IEnumerator ShootingCountDown()
    {
        canShoot = false;
        yield return new WaitForSeconds(weaponCountDown);
        canShoot = true;
    }
}
