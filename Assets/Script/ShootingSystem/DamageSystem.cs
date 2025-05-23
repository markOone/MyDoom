using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDoom.Player;

namespace MyDoom.ShootingSystem
{
    public class DamageSystem : MonoBehaviour, IDamageHandler
    {
        [Header("Effects")] [SerializeField] private GameObject enemyImpactEffect;
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
            Debug.Log("HandleDamage");
            if (context.MultipleHits != null)
            {
                Debug.Log("MultipleHits");
                HandleMultipleHits(context);
                return;
            }

            Debug.Log("SingleHit");
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

            Debug.Log("HandleMultipleHits");

            bool hitEnemyAtLevel = false;
            List<RaycastHit> nonEnemyHits = new List<RaycastHit>();

            foreach (var hitInfo in context.MultipleHits)
            {
                if (IsFireballLayer(hitInfo.transform.gameObject))
                    continue;

                var damageable = hitInfo.transform.gameObject.GetComponent<IDamagable>();
                if (damageable != null)
                {
                    hitEnemyAtLevel = true;
                    ApplyDamage(new DamageContext
                    {
                        HitInfo = hitInfo,
                        GunData = context.GunData
                    }, damageable);
                    Debug.Log("ДІСТАЛИСЬ ДО СПАВНУ");
                    SpawnImpactEffect(enemyImpactEffect, hitInfo);
                }
                else
                {
                    nonEnemyHits.Add(hitInfo);
                }
            }

            if (hitEnemyAtLevel)
            {
                return;
            }

            if (context.AutoAimAllowed)
            {
                Debug.Log("АВТО АІМ");
                bool autoAimFoundEnemy = TryAutoAim(context);

                if (!autoAimFoundEnemy && nonEnemyHits.Count > 0)
                {
                    foreach (var hitInfo in nonEnemyHits)
                    {
                        Debug.Log("СПАВН МЕТАЛУ ПІСЛЯ НЕВДАЛОГО АВТОАІМУ");
                        SpawnImpactEffect(metalImpactEffect, hitInfo);
                    }
                }
            }
            else
            {
                foreach (var hitInfo in nonEnemyHits)
                {
                    Debug.Log("СПАВН МЕТАЛУ БЕЗ АВТОАІМУ");
                    SpawnImpactEffect(metalImpactEffect, hitInfo);
                }
            }
        }

        private void HandleAutoAim(DamageContext context)
        {
            bool foundEnemy = TryAutoAim(context);
            
            if (!foundEnemy)
            {
                if (context.HitInfo.transform != null)
                {
                    //Debug.Log("СПАВН МЕТАЛУ ПІСЛЯ НЕВДАЛОГО АВТОАІМУ (SINGLE SHOT)");
                    SpawnImpactEffect(metalImpactEffect, context.HitInfo);
                }
                else if (context.MultipleHits != null && context.MultipleHits.Count > 0)
                {
                    foreach (var hit in context.MultipleHits)
                    {
                        //Debug.Log("СПАВН МЕТАЛУ ПІСЛЯ НЕВДАЛОГО АВТОАІМУ (MULTIPLE HITS)");
                        SpawnImpactEffect(metalImpactEffect, hit);
                    }
                }
            }
        }
        
        private bool TryAutoAim(DamageContext context)
        {
            GameObject targetEnemy = _autoAimSystem.ChooseEnemyWithAutoAim(new AutoAimContext
            {
                Origin = context.Origin,
                GunData = context.GunData,
                FieldOfView = 60f,   
                RayCount = 15        
            });
            
            if (targetEnemy != null)
            {
                Vector3 directionToEnemy = (targetEnemy.transform.position - context.Origin.position).normalized;
                
                var autoAimContext = new WeaponContext
                {
                    Origin = context.Origin,
                    GunData = context.GunData,
                    AutoAimAllowed = false, 
                    Direction = directionToEnemy
                };
                
                if (Physics.Raycast(context.Origin.position, directionToEnemy, out RaycastHit enemyHit, context.GunData.lengthRange))
                {
                    var damageable = enemyHit.transform.gameObject.GetComponent<IDamagable>();
                    if (damageable != null)
                    {
                        Debug.Log("АВТОАІМ: влучання у ворога!");
                        ApplyDamage(new DamageContext 
                        { 
                            HitInfo = enemyHit, 
                            GunData = context.GunData 
                        }, damageable);
                        SpawnImpactEffect(enemyImpactEffect, enemyHit);
                    }
                }
                return true; 
            }
    
            return false; 
        }
        
        private void SpawnImpactEffect(GameObject effectPrefab, RaycastHit hit)
        {
            Vector3 normal = hit.normal;
            if (normal == Vector3.zero)
            {
                normal = (hit.transform.position - hit.point).normalized;

                if (normal == Vector3.zero)
                {
                    normal = Vector3.up;
                }
            }

            var effect = Instantiate(
                effectPrefab,
                hit.point,
                Quaternion.LookRotation(normal)
            );
            StartCoroutine(DestroyAfterDelay(effect, 1f));
        }

        private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(obj);
        }
    }
}