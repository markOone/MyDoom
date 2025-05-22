using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDoom.Player;

namespace MyDoom.ShootingSystem
{
    public class DamageSystem : MonoBehaviour, IDamageHandler
    {
        [Header("Effects")]
        [SerializeField] private GameObject enemyImpactEffect;
        [SerializeField] private GameObject metalImpactEffect;
        
        private IDamageHandler _damageHandler;
        private IAutoAimSystem _autoAimSystem;

        private void Awake()
        {
            _damageHandler = GetComponent<IDamageHandler>();
            _autoAimSystem = GetComponent<IAutoAimSystem>();
        }

        private static DamageSystem _instance;

        public static DamageSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log("No DamageSystem");
                }

                return _instance;
            }
        }

        private void OnEnable()
        {
            _instance = this;
        }
        
        public void HandleDamage(DamageContext context)
        {
            if (context.MultipleHits != null)
            {
                HandleMultipleHits(context);
                return;
            }

            HandleSingleHit(context);
        }
        
        private void HandleSingleHit(DamageContext context)
        {
            if (IsFireballLayer(context.HitInfo.transform.gameObject)) return;

            var damageable = context.HitInfo.transform.GetComponent<IDamagable>();
            if (damageable != null)
            {
                ApplyDamage(context, damageable);
                SpawnImpactEffect(enemyImpactEffect, context.HitInfo);
            }
            else if (context.AutoAimAllowed)
            {
                HandleAutoAim(context);
            }
            else
            {
                SpawnImpactEffect(metalImpactEffect, context.HitInfo);
            }
        }

        private void ApplyDamage(DamageContext context, IDamagable damageable)
        {
            damageable.Damage(context.GunData.damage, context.HitInfo.distance);
        }

        bool IsFireballLayer(GameObject gameObject)
        {
            return gameObject.layer == LayerMask.NameToLayer("FireBall");
        }
        
        private void HandleMultipleHits(DamageContext context)
        {
            if (context.MultipleHits.Count == 0) return;

            bool hitEnemy = false;
            List<RaycastHit> nonEnemyHits = new List<RaycastHit>();
            
            foreach (var hitInfo in context.MultipleHits)
            {
                if (IsFireballLayer(hitInfo.transform.gameObject))
                    continue;

                var damageable = hitInfo.transform.gameObject.GetComponent<IDamagable>();
                if (damageable != null)
                {
                    hitEnemy = true;
                    ApplyDamage(new DamageContext 
                    { 
                        HitInfo = hitInfo, 
                        GunData = context.GunData 
                    }, damageable);
                    SpawnImpactEffect(enemyImpactEffect, hitInfo);
                }
                else
                {
                    nonEnemyHits.Add(hitInfo);
                }
            }
            
            if (!hitEnemy)
            {
                if (context.AutoAimAllowed)
                {
                    HandleAutoAim(context);
                }
                else
                {
                    foreach (var hitInfo in nonEnemyHits)
                    {
                        SpawnImpactEffect(metalImpactEffect, hitInfo);
                    }
                }
            }
        }
        
        private void SpawnImpactEffect(GameObject effectPrefab, RaycastHit hit)
        {
            var effect = Instantiate(
                effectPrefab,
                hit.point,
                Quaternion.LookRotation(hit.normal)
            );
            StartCoroutine(DestroyAfterDelay(effect, 1f));
        }
        
        private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(obj);
        }
        
        private void HandleAutoAim(DamageContext context)
        {
            // 1. Try to find nearest enemy using AutoAimSystem
            GameObject targetEnemy = _autoAimSystem.ChooseEnemyWithAutoAim(new AutoAimContext
            {
                Origin = context.Origin,
                GunData = context.GunData,
                FieldOfView = 60f,   // Angle to search for enemies
                RayCount = 15        // Number of rays to cast for enemy detection
            });

            // 2. If we found an enemy
            if (targetEnemy != null)
            {
                // 3. Calculate direction to the enemy
                Vector3 directionToEnemy = (targetEnemy.transform.position - context.Origin.position).normalized;

                // 4. Prepare new context for auto-aimed shot
                var autoAimContext = new WeaponContext
                {
                    Origin = context.Origin,
                    GunData = context.GunData,
                    AutoAimAllowed = false,  // Prevent infinite auto-aim loops
                    Direction = directionToEnemy
                };

                // 5. Get the appropriate weapon system based on gun type
                IWeaponSystem weaponSystem;
                if (context.GunData.shells)
                {
                    // For shotgun
                    weaponSystem = GetComponent<HitScanShooting>();
                }
                else
                {
                    // For other weapons
                    weaponSystem = GetComponent<ProjectileShooting>();
                }
                
                weaponSystem?.Shoot(autoAimContext);
            }
            else
            {
                // If no enemy found, just show impact effect
                SpawnImpactEffect(metalImpactEffect, context.HitInfo);
            }
        }


        // public void HandleHit(RaycastHit hitInfo,
        //     bool isAutoAimAlllowed,
        //     GunData gunData,
        //     Transform originForAutoAim)
        // {
        //     if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("FireBall")) return;
        //
        //     IDamagable? damagable = hitInfo.transform.gameObject.GetComponent<IDamagable>();
        //     GameObject impactEffect;
        //
        //     if (damagable != null)
        //     {
        //         damagable.Damage(gunData.damage, hitInfo.distance);
        //         impactEffect = Instantiate(PlayerShooting.Instance.enemyImpactEffect, hitInfo.point,
        //             Quaternion.LookRotation(hitInfo.normal));
        //         StartCoroutine(DestroyEffectTwo(impactEffect, 1));
        //     }
        //     else
        //     {
        //         if (isAutoAimAlllowed)
        //         {
        //             GameObject targetEnemy = AutoAimSystem.Instance.ChooseEnemyWithAutoAim(originForAutoAim, gunData);
        //             if (targetEnemy != null)
        //             {
        //                 //HitScanShooting.ShootSingleRayWithAutoAim(originForAutoAim, gunData, targetEnemy);
        //                 return;
        //             }
        //
        //             impactEffect = Instantiate(PlayerShooting.Instance.metalImpactEffect, hitInfo.point,
        //                 Quaternion.LookRotation(hitInfo.normal));
        //             StartCoroutine(DestroyEffectTwo(impactEffect, 1));
        //         }
        //     }
        // }

        // public void HandleHit(List<RaycastHit> hitInfoArray,
        //     bool isAutoAimAlllowed,
        //     GunData gunData,
        //     Transform originForAutoAim)
        // {
        //     if (hitInfoArray.Count == 0) return;
        //
        //     bool hitEnemy = false;
        //     List<RaycastHit> nonEnemyHits = new List<RaycastHit>();
        //
        //     foreach (var hitInfo in hitInfoArray)
        //     {
        //         if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("FireBall"))
        //             continue;
        //
        //         IDamagable? damagable = hitInfo.transform.gameObject.GetComponent<IDamagable>();
        //
        //         if (damagable != null)
        //         {
        //             hitEnemy = true;
        //             damagable.Damage(gunData.damage, hitInfo.distance);
        //             GameObject impactEffect = Instantiate(PlayerShooting.Instance.enemyImpactEffect,
        //                 hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        //             StartCoroutine(DestroyEffectTwo(impactEffect, 1));
        //         }
        //         else
        //         {
        //             nonEnemyHits.Add(hitInfo);
        //         }
        //     }
        //
        //     if (!hitEnemy)
        //     {
        //         if (isAutoAimAlllowed)
        //         {
        //             // Try auto-aim
        //             GameObject targetEnemy = AutoAimSystem.Instance.ChooseEnemyWithAutoAim(originForAutoAim, gunData);
        //             if (targetEnemy != null)
        //             {
        //                 HitScanShooting.ShootShotGunWithAutoAim(originForAutoAim, gunData, targetEnemy);
        //                 return;
        //             }
        //         }
        //
        //         foreach (var hitInfo in nonEnemyHits)
        //         {
        //             GameObject impactEffect = Instantiate(PlayerShooting.Instance.metalImpactEffect,
        //                 hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        //             StartCoroutine(DestroyEffectTwo(impactEffect, 1));
        //         }
        //     }
        // }
        //
        
    }
}