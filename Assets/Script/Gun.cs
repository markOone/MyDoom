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
    [SerializeField] AudioSource audioSource;

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    internal void Shoot()
    {
	    if (gunData.currentAmmo > 0 && CanShoot())
	    {
		    audioSource?.Play(0);
		    colliders = Physics.OverlapSphere(transform.position, shotSoundRadius, enemyLayerMask);

		    foreach (var enemy in colliders)
		    {
			    enemy.GetComponent<Enemy>().playerInSightRange = true;
		    }

		    if (gunData.name == "Shotgun")
		    {
			    ShootShotgun();
		    }
		    else
		    {
			    ShootSingleRay();
		    }

		    if (gunData.is2D)
		    {
			    PlayerShooting.Instance.Shoot2D();
		    }

		    gunData.currentAmmo--;
		    UpdateAmmoCounters();
		    timeSinceLastShot = 0;
		    OnGunShot();
		    HudController.Instance.UpdateHUD();
	    }
    }

    private void ShootShotgun()
    {
	    Vector3[] offsets =
	    {
		    Vector3.zero,
		    new Vector3(0.01f, 0f, 0f),
		    new Vector3(-0.01f, 0f, 0f),
		    new Vector3(0.02f, 0f, 0f),
		    new Vector3(-0.02f, 0f, 0f),
		    new Vector3(0.03f, 0f, 0f),
		    new Vector3(-0.03f, 0f, 0f)
	    };

	    foreach (var offset in offsets)
	    {
		    if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward + offset,
			        out RaycastHit hitInfo, gunData.lengthRange))
		    {
			    HandleHit(hitInfo);
		    }
	    }
    }

    private void ShootSingleRay()
    {
	    PlayerShooting.Instance.muzzleFlashEffect.Play();
	    if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out RaycastHit hitInfo,
		        gunData.lengthRange))
	    {
		    HandleHit(hitInfo);
	    }
    }

    private void HandleHit(RaycastHit hitInfo)
    {
	    IDamagable damagable = hitInfo.transform.gameObject.GetComponent<IDamagable>();
	    GameObject impactEffect;

	    if (damagable != null)
	    {
		    damagable.Damage(gunData.damage);
		    impactEffect = Instantiate(PlayerShooting.Instance.enemyImpactEffect, hitInfo.point,
			    Quaternion.LookRotation(hitInfo.normal));
	    }
	    else
	    {
		    impactEffect = Instantiate(PlayerShooting.Instance.metalImpactEffect, hitInfo.point,
			    Quaternion.LookRotation(hitInfo.normal));
	    }

	    DestroyEffect(impactEffect);
	    Debug.Log(hitInfo.transform.gameObject.name);
	    Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
    }

    private void UpdateAmmoCounters()
    {
	    if (gunData.shells) PlayerStats.Instance.ShellCounter--;
	    if (gunData.bullets) PlayerStats.Instance.BulletsCounter--;
	    if (gunData.rockets) PlayerStats.Instance.RocketsCounter--;
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
