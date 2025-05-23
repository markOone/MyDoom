using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using MyDoom.Enemies;
using MyDoom.GeneralSystems;
using MyDoom.Player;
using Object = System.Object;


namespace MyDoom.ShootingSystem
{
    public class Gun : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] GameObject ShootingServices;
        private IWeaponSystem _weaponSystem;
        [SerializeField] internal GunData gunData;

        [SerializeField] GameObject particlePrefab;
        [SerializeField] Transform particleSpawnPoint;
        [SerializeField] Transform shooterOrigin;

        [SerializeField] Camera fpsCamera;
        float timeSinceLastShot;
        [SerializeField] private float shotSoundRadius;
        [SerializeField] private LayerMask enemyLayerMask;
        Collider[] colliders;
        [SerializeField] AudioSource audioSource;

        private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

        private void Start()
        {
            InitializeWeaponSystem();
            SubscribeToEvents();
            CheckAmmoFromPlayerStats();
        }
        
        private void OnDisable()
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnAmmoChanged -= UpdateAmmoFromPlayerStats;
            }
        }

        internal void Shoot()
        {
            if (gunData.currentAmmo <= 0 || !CanShoot() || gunData.name == "Hand")
                return;
            
            AlertNearbyEnemies();
            
            var context = new WeaponContext
            {
                Origin = shooterOrigin,
                GunData = gunData,
                AutoAimAllowed = true,
                ProjectilePrefab = particlePrefab
            };
            
            Debug.Log("Shooting with " + gunData.name);
            _weaponSystem.Shoot(context);
            
            HandlePostShot();
        }
        
        private void AlertNearbyEnemies()
        {
            int numColliders = Physics.OverlapSphereNonAlloc(
                transform.position, 
                shotSoundRadius, 
                colliders, 
                enemyLayerMask
            );

            for (int i = 0; i < numColliders; i++)
            {
                if (colliders[i] == null) continue;
                
                if (colliders[i].TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.playerInSightRange = true;
                }
            }
        }
        
        private void SubscribeToEvents()
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnAmmoChanged += UpdateAmmoFromPlayerStats;
            }
        }
        
        private void HandlePostShot()
        {
            audioSource?.Play();
            
            if (gunData.is2D)
            {
                PlayerShooting.Instance.Shoot2D();
            }
            
            gunData.currentAmmo--;
            UpdateAmmoCounters();
            timeSinceLastShot = 0;
            OnGunShot();
        }
        
        private void InitializeWeaponSystem()
        {
            if(gunData.name == "Hand") _weaponSystem = ShootingServices.GetComponent<HitScanShooting>();
            // Get appropriate weapon system based on gun type
            if (gunData.shells)
            {
                _weaponSystem = ShootingServices.GetComponent<HitScanShooting>();
            }
            else if (gunData.rockets || gunData.cells)
            {
                _weaponSystem = ShootingServices.GetComponent<ProjectileShooting>();
            }
            else if (gunData.bullets)
            {
                _weaponSystem = ShootingServices.GetComponent<HitScanShooting>();
            }
            
            if (_weaponSystem == null)
            {
                Debug.LogError($"No weapon system found for gun: {gunData.name}");
            }
        }
        
        private void UpdateAmmoCounters()
        {
            if (gunData.shells) PlayerStats.Instance.DecreaseAmmo(1, AmmoType.Shell);
            if (gunData.bullets) PlayerStats.Instance.DecreaseAmmo(1, AmmoType.Bullet);
            if (gunData.rockets) PlayerStats.Instance.DecreaseAmmo(1, AmmoType.Rocket);
            if (gunData.cells) PlayerStats.Instance.DecreaseAmmo(1, AmmoType.Cell);
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

        public void UpdateAmmoFromPlayerStats([CanBeNull] object sender, AmmoChangedEventArgs e)
        {
            if (gunData.shells && e.Type == AmmoType.Shell) gunData.currentAmmo = e.Ammo;
            if(gunData.bullets && e.Type == AmmoType.Bullet) gunData.currentAmmo = e.Ammo;
            if(gunData.rockets && e.Type == AmmoType.Rocket) gunData.currentAmmo = e.Ammo;
            if(gunData.cells && e.Type == AmmoType.Cell) gunData.currentAmmo = e.Ammo;
        }
        
        public void CheckAmmoFromPlayerStats()
        {
            if(gunData.shells) gunData.currentAmmo = PlayerStats.Instance.ShellCounter;
            if(gunData.bullets) gunData.currentAmmo = PlayerStats.Instance.BulletsCounter;
            if(gunData.rockets) gunData.currentAmmo = PlayerStats.Instance.RocketsCounter;
            if(gunData.cells) gunData.currentAmmo = PlayerStats.Instance.CellsCounter;
        }
    }
}