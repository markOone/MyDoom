using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] internal GunData gunData;
    [SerializeField] Camera fpsCamera;
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

                if (gunData.name == "Shotgun")
                {
                    Vector3 offset = new Vector3(0.1f, 0f, 0f);
                    if (Physics.Raycast(fpsCamera.transform.position + offset, fpsCamera.transform.forward, out RaycastHit hitInfo1,
                            100f))
                    {
                        IDamagable damagable = hitInfo1.transform.gameObject.GetComponent<IDamagable>();
                        damagable?.Damage(gunData.damage);
                        Debug.Log(hitInfo1.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
                    }
                    
                    if(Physics.Raycast(fpsCamera.transform.position - offset, fpsCamera.transform.forward, out RaycastHit hitInfo2,
                           100f))
                    {
                        IDamagable damagable = hitInfo2.transform.gameObject.GetComponent<IDamagable>();
                        damagable?.Damage(gunData.damage);
                        Debug.Log(hitInfo1.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
                    }
                    
                    if(Physics.Raycast(fpsCamera.transform.position + (offset * 2), fpsCamera.transform.forward, out RaycastHit hitInfo3,
                           100f))
                    {
                        IDamagable damagable = hitInfo3.transform.gameObject.GetComponent<IDamagable>();
                        damagable?.Damage(gunData.damage);
                        Debug.Log(hitInfo1.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
                    }
                    
                    if(Physics.Raycast(fpsCamera.transform.position + (offset * -2), fpsCamera.transform.forward, out RaycastHit hitInfo4,
                           100f))
                    {
                        IDamagable damagable = hitInfo4.transform.gameObject.GetComponent<IDamagable>();
                        damagable?.Damage(gunData.damage);
                        Debug.Log(hitInfo1.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
                    }
                    
                    if(Physics.Raycast(fpsCamera.transform.position + (offset * 3), fpsCamera.transform.forward, out RaycastHit hitInfo5,
                           100f))
                    {
                        IDamagable damagable = hitInfo5.transform.gameObject.GetComponent<IDamagable>();
                        damagable?.Damage(gunData.damage);
                        Debug.Log(hitInfo1.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
                    }
                    
                    if(Physics.Raycast(fpsCamera.transform.position + (offset * -3), fpsCamera.transform.forward, out RaycastHit hitInfo6,
                           100f))
                    {
                        IDamagable damagable = hitInfo6.transform.gameObject.GetComponent<IDamagable>();
                        damagable?.Damage(gunData.damage);
                        Debug.Log(hitInfo1.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
                    }
                }
                else
                {
                    if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out RaycastHit hitInfo, gunData.lengthRange))
                    {
                        IDamagable damagable = hitInfo.transform.gameObject.GetComponent<IDamagable>();
                        damagable?.Damage(gunData.damage);
                        Debug.Log(hitInfo.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
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
        if(gunData.cells) gunData.currentAmmo = PlayerStats.Instance.CellsCounter;
    }
}
