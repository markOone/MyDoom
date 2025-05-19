using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDoom.Enemies;
using MyDoom.Player;


namespace MyDoom.ShootingSystem
{
    public class Gun : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        internal GunData gunData;
        [SerializeField] GameObject particlePrefab;
        [SerializeField] Transform particleSpawnPoint;

        [SerializeField] Camera fpsCamera;
        float timeSinceLastShot;
        [SerializeField] private float shotSoundRadius;
        [SerializeField] private LayerMask enemyLayerMask;
        Collider[] colliders;
        [SerializeField] AudioSource audioSource;

        private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

        internal void Shoot()
        {
            if (gunData.currentAmmo > 0 && CanShoot())
            {
                audioSource?.Play(0);
                colliders = Physics.OverlapSphere(transform.position, shotSoundRadius, enemyLayerMask);

                foreach (var enemy in colliders)
                {
                    enemy.gameObject.GetComponent<Enemy>().playerInSightRange = true;
                }

                if (gunData.name == "Shotgun")
                {
                    ShootShotgun();
                }
                
                if (gunData.rockets || gunData.cells)
                { 
                    ShootSingleRay(ShootingType.Particle);
                }
                
                if(gunData.bullets)
                { 
                    ShootSingleRay(ShootingType.HitScan);    
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

            List<RaycastHit> raycastHits = new List<RaycastHit>();

            foreach (var offset in offsets)
            {
                Vector3 spreadDirection = fpsCamera.transform.forward
                                          + fpsCamera.transform.right * offset.x
                                          + fpsCamera.transform.up * offset.y;
                spreadDirection.Normalize();

                if (Physics.Raycast(fpsCamera.transform.position, spreadDirection, out RaycastHit hitInfo,
                        gunData.lengthRange))
                {
                    raycastHits.Add(hitInfo);
                }
            }

            HandleHit(raycastHits, false);
        }

        private void ShootShotgunWithAutoAim(GameObject enemy)
        {
            Vector3 origin = fpsCamera.transform.position;
            Vector3 direction = (enemy.transform.position - origin).normalized;

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

            List<RaycastHit> raycastHits = new List<RaycastHit>();

            foreach (var offset in offsets)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                Vector3 spreadDirection = rotation * (Vector3.forward + offset);
                spreadDirection.Normalize();

                Debug.DrawRay(fpsCamera.transform.position, spreadDirection * gunData.lengthRange, Color.green, 0.1f);

                if (Physics.Raycast(fpsCamera.transform.position, spreadDirection, out RaycastHit hitInfo,
                        gunData.lengthRange))
                {
                    raycastHits.Add(hitInfo);
                }
            }

            HandleHit(raycastHits, true);
        }

        private void ShootSingleRay(ShootingType type)
        {
            PlayerShooting.Instance.muzzleFlashEffect.Play();
            if (type == ShootingType.Particle)
            {
                Vector3 spawnPoint = particleSpawnPoint.position; 
                //spawnPoint.z -= 5f;
                Rigidbody rb = Instantiate(particlePrefab, spawnPoint, particleSpawnPoint.rotation)
                    .GetComponent<Rigidbody>();
                //Vector3 spawnPoint = GameManager.Instance.playerPosition.forward;
                //spawnPoint.z += 1f;
                rb.gameObject.GetComponent<ProjectileScript>().Damage = gunData.damage;
                //spawnPoint.y += 1f;
                rb.AddForce(particleSpawnPoint.forward * 200f, ForceMode.Impulse);
                //rb.AddForce(transform.up * 4f, ForceMode.Impulse);
            }
            else
            {
                if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out RaycastHit hitInfo,
                        gunData.lengthRange))
                {
                    HandleHit(hitInfo, false);
                }    
            }
            
        }

        private void ShootWithAutoAim(GameObject enemy)
        {
            Vector3 origin = fpsCamera.transform.position;
            Vector3 direction = (enemy.transform.position - origin).normalized;

            PlayerShooting.Instance.muzzleFlashEffect.Play();
            if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, gunData.lengthRange))
            {
                HandleHit(hitInfo, true);
            }
        }

        private void HandleHit(List<RaycastHit> hitInfoArray, bool isAuto)
        {
            if (hitInfoArray.Count == 0) return;

            bool hitEnemy = false;
            List<RaycastHit> nonEnemyHits = new List<RaycastHit>();

            foreach (var hitInfo in hitInfoArray)
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("FireBall"))
                    continue;

                IDamagable damagable = hitInfo.transform.gameObject.GetComponent<IDamagable>();

                if (damagable != null)
                {
                    hitEnemy = true;
                    damagable.Damage(gunData.damage, hitInfo.distance);
                    GameObject impactEffect = Instantiate(PlayerShooting.Instance.enemyImpactEffect,
                        hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    StartCoroutine(DestroyEffectTwo(impactEffect, 1));
                }
                else
                {
                    nonEnemyHits.Add(hitInfo);
                }
            }

            if (!hitEnemy)
            {
                if (!isAuto)
                {
                    // Try auto-aim
                    GameObject targetEnemy = ChooseEnemyWithAutoAim();
                    if (targetEnemy != null)
                    {
                        ShootShotgunWithAutoAim(targetEnemy);
                        return;
                    }
                }

                foreach (var hitInfo in nonEnemyHits)
                {
                    GameObject impactEffect = Instantiate(PlayerShooting.Instance.metalImpactEffect,
                        hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    StartCoroutine(DestroyEffectTwo(impactEffect, 1));
                }
            }
        }

        private void HandleHit(RaycastHit hitInfo, bool isAuto)
        {
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("FireBall")) return;

            IDamagable damagable = hitInfo.transform.gameObject.GetComponent<IDamagable>();
            GameObject impactEffect;

            if (damagable != null)
            {
                damagable.Damage(gunData.damage, hitInfo.distance);
                impactEffect = Instantiate(PlayerShooting.Instance.enemyImpactEffect, hitInfo.point,
                    Quaternion.LookRotation(hitInfo.normal));
                StartCoroutine(DestroyEffectTwo(impactEffect, 1));
            }
            else
            {
                if (!isAuto)
                {
                    GameObject targetEnemy = ChooseEnemyWithAutoAim();
                    if (targetEnemy != null)
                    {
                        ShootWithAutoAim(targetEnemy);
                        return;
                    }

                    impactEffect = Instantiate(PlayerShooting.Instance.metalImpactEffect, hitInfo.point,
                        Quaternion.LookRotation(hitInfo.normal));
                    StartCoroutine(DestroyEffectTwo(impactEffect, 1));
                }
            }

            Debug.Log(hitInfo.transform.gameObject.name);
            Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * 100f, Color.red);
        }

        private GameObject ChooseEnemyWithAutoAim()
        {
            List<KeyValuePair<float, GameObject>> enemiesInDirection = new();

            int rayCount = 15;
            float fieldOfView = 60f;
            float angleStep = fieldOfView / (rayCount - 1);

            for (int i = 0; i <= rayCount; i++)
            {
                float angle = -fieldOfView / 2 + i * angleStep;
                Vector3 direction = fpsCamera.transform.rotation * Quaternion.Euler(angle, 0, 0) * Vector3.forward;
                direction.Normalize();

                Debug.DrawRay(fpsCamera.transform.position, direction * gunData.lengthRange, Color.yellow, 0.1f);

                if (Physics.Raycast(fpsCamera.transform.position, direction, out RaycastHit hitInfo,
                        gunData.lengthRange))
                {
                    if (hitInfo.collider.CompareTag("Enemy"))
                    {
                        enemiesInDirection.Add(
                            new KeyValuePair<float, GameObject>(hitInfo.distance, hitInfo.transform.gameObject));
                    }
                }
            }

            GameObject closest = null;
            float smallestDistance = float.MaxValue;

            foreach (var obj in enemiesInDirection)
            {
                if (obj.Key < smallestDistance)
                {
                    smallestDistance = obj.Key;
                    closest = obj.Value;
                }
            }

            return closest;
        }

        private void UpdateAmmoCounters()
        {
            if (gunData.shells) PlayerStats.Instance.ShellCounter--;
            if (gunData.bullets) PlayerStats.Instance.BulletsCounter--;
            if (gunData.rockets) PlayerStats.Instance.RocketsCounter--;
            if(gunData.cells) PlayerStats.Instance.CellsCounter--;
        }

        public void DestroyEffect(GameObject effect)
        {
            Destroy(effect, 1f);
        }

        IEnumerator DestroyEffectTwo(GameObject effect, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(effect);
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
            if (gunData.rockets) gunData.currentAmmo = PlayerStats.Instance.RocketsCounter;
            if (gunData.cells) gunData.currentAmmo = PlayerStats.Instance.CellsCounter;
        }
    }

    public enum ShootingType
    {
        HitScan,
        Particle
    }
}