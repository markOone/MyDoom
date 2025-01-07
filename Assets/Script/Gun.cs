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
    [SerializeField] private float shotSoundRadius;
    [SerializeField] private LayerMask enemyLayerMask;
    Collider[] colliders;

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
                colliders = Physics.OverlapSphere(transform.position, shotSoundRadius, enemyLayerMask);

                foreach (var enemy in colliders)
                {
                    enemy.GetComponent<Enemy>().playerInSightRange = true;
                }

                if (gunData.name == "Shotgun")
                {
                    Vector3 offset = new Vector3(0.01f, 0f, 0f);
                    
                    if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out RaycastHit hitInfo, gunData.lengthRange))
                    {
                        IDamagable damagable = hitInfo.transform.gameObject.GetComponent<IDamagable>();
                        if (damagable != null)
                        {
                            damagable.Damage(gunData.damage);
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.enemyImpactEffect, hitInfo.point,
                                Quaternion.LookRotation(hitInfo.normal));
                            DestroyEffect(impactEffect);
                        }else
                        {
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.metalImpactEffect, hitInfo.point,
                                Quaternion.LookRotation(hitInfo.normal));
                            DestroyEffect(impactEffect);
                        }
                        Debug.Log(hitInfo.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
                    }
                    
                    if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward + offset, out RaycastHit hitInfo1,
                            100f))
                    {
                        IDamagable damagable = hitInfo1.transform.gameObject.GetComponent<IDamagable>();
                        if (damagable != null)
                        {
                            damagable.Damage(gunData.damage);
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.enemyImpactEffect, hitInfo1.point,
                                Quaternion.LookRotation(hitInfo1.normal));
                            DestroyEffect(impactEffect);
                        }
                        else
                        {
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.metalImpactEffect, hitInfo1.point,
                                Quaternion.LookRotation(hitInfo1.normal));
                            DestroyEffect(impactEffect);
                        }
                        Debug.Log(hitInfo1.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
                    }
                    
                    if(Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward - offset, out RaycastHit hitInfo2,
                           100f))
                    {
                        IDamagable damagable = hitInfo2.transform.gameObject.GetComponent<IDamagable>();
                        if (damagable != null)
                        {
                            damagable.Damage(gunData.damage);
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.enemyImpactEffect, hitInfo2.point,
                                Quaternion.LookRotation(hitInfo2.normal));
                            DestroyEffect(impactEffect);
                        }else
                        {
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.metalImpactEffect, hitInfo2.point,
                                Quaternion.LookRotation(hitInfo2.normal));
                            DestroyEffect(impactEffect);
                        }
                        Debug.Log(hitInfo2.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
                    }
                    
                    if(Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward + (offset * 2), out RaycastHit hitInfo3,
                           100f))
                    {
                        IDamagable damagable = hitInfo3.transform.gameObject.GetComponent<IDamagable>();
                        if (damagable != null)
                        {
                            damagable.Damage(gunData.damage);
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.enemyImpactEffect, hitInfo3.point,
                                Quaternion.LookRotation(hitInfo3.normal));
                            DestroyEffect(impactEffect);
                        }else
                        {
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.metalImpactEffect, hitInfo3.point,
                                Quaternion.LookRotation(hitInfo3.normal));
                            DestroyEffect(impactEffect);
                        }
                        Debug.Log(hitInfo3.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
                    }
                    
                    if(Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward + (offset * -2), out RaycastHit hitInfo4,
                           100f))
                    {
                        IDamagable damagable = hitInfo4.transform.gameObject.GetComponent<IDamagable>();
                        if (damagable != null)
                        {
                            damagable.Damage(gunData.damage);
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.enemyImpactEffect, hitInfo4.point,
                                Quaternion.LookRotation(hitInfo4.normal));
                            DestroyEffect(impactEffect);
                        }else
                        {
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.metalImpactEffect, hitInfo4.point,
                                Quaternion.LookRotation(hitInfo4.normal));
                            DestroyEffect(impactEffect);
                        }
                        Debug.Log(hitInfo4.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
                    }
                    
                    if(Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward + (offset * 3), out RaycastHit hitInfo5,
                           100f))
                    {
                        IDamagable damagable = hitInfo5.transform.gameObject.GetComponent<IDamagable>();
                        if (damagable != null)
                        {
                            damagable.Damage(gunData.damage);
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.enemyImpactEffect, hitInfo5.point,
                                Quaternion.LookRotation(hitInfo5.normal));
                            DestroyEffect(impactEffect);
                        }else
                        {
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.metalImpactEffect, hitInfo5.point,
                                Quaternion.LookRotation(hitInfo5.normal));
                            DestroyEffect(impactEffect);
                        }
                        Debug.Log(hitInfo5.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
                    }
                    
                    if(Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward + (offset * -3), out RaycastHit hitInfo6,
                           100f))
                    {
                        IDamagable damagable = hitInfo6.transform.gameObject.GetComponent<IDamagable>();
                        if (damagable != null)
                        {
                            damagable.Damage(gunData.damage);
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.enemyImpactEffect, hitInfo6.point,
                                Quaternion.LookRotation(hitInfo6.normal));
                            DestroyEffect(impactEffect);
                        }else
                        {
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.metalImpactEffect, hitInfo6.point,
                                Quaternion.LookRotation(hitInfo6.normal));
                            DestroyEffect(impactEffect);
                        }
                        Debug.Log(hitInfo6.transform.gameObject.name);
                        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
                    }
                }
                else
                {
                    PlayerShooting.Instance.muzzleFlashEffect.Play();
                    if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out RaycastHit hitInfo, gunData.lengthRange))
                    {
                        IDamagable damagable = hitInfo.transform.gameObject.GetComponent<IDamagable>();
                        if (damagable != null)
                        {
                            damagable.Damage(gunData.damage);
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.enemyImpactEffect, hitInfo.point,
                                Quaternion.LookRotation(hitInfo.normal));
                            DestroyEffect(impactEffect);
                        }else
                        {
                            GameObject impactEffect = Instantiate(PlayerShooting.Instance.metalImpactEffect, hitInfo.point,
                                Quaternion.LookRotation(hitInfo.normal));
                            DestroyEffect(impactEffect);
                        }
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

    public void DestroyEffect(GameObject effect)
    {
        Destroy(effect, 1f);
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
