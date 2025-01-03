using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] internal GunData gunData;
    float timeSinceLastShot;

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
    void Start()
    {
        
    }

    public void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (PlayerShooting.Instance.shootingField.EnemiesInField.Count > 0);
                {
                    foreach (GameObject obj in PlayerShooting.Instance.shootingField.EnemiesInField)
                    {
                        IDamagable damagable = obj.GetComponent<IDamagable>();
                        damagable?.Damage(gunData.damage);
                    }
                }

                if (gunData.is2D)
                {
                    PlayerShooting.Instance.Shoot2D();
                }
                
                gunData.currentAmmo--;

                if (gunData.shells) PlayerStats.Instance.ShellCounter--;
                if (gunData.bullets) PlayerStats.Instance.BulletsCounter--;
                if(gunData.rockets) PlayerStats.Instance.RocketsCounter--;
                
                timeSinceLastShot = 0;
                OnGunShot();
                HudController.Instance.UpdateHUD();
            }
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    private void OnGunShot()
    {
        
    }

    public void CheckAmmo()
    {
        if (gunData.shells) gunData.currentAmmo = PlayerStats.Instance.ShellCounter;
        if (gunData.bullets) gunData.currentAmmo = PlayerStats.Instance.BulletsCounter;
        if(gunData.rockets) gunData.currentAmmo = PlayerStats.Instance.RocketsCounter;
    }
}
