using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyDoom.ShootingSystem
{
    public class HitScanShooting : MonoBehaviour, IWeaponSystem
    {
        private IDamageHandler _damageHandler;
        private IAutoAimSystem _autoAimSystem;
        
        private static HitScanShooting _instance;
        
        public static HitScanShooting Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log("No HitScanShooting");
                }

                return _instance;
            }
        }
        
        
        private void Awake()
        {
            _damageHandler = GetComponent<IDamageHandler>();
            _autoAimSystem = GetComponent<IAutoAimSystem>();
        }

        private void OnEnable()
        {
            _instance = this;
        }


        public void Shoot(WeaponContext context)
        {
            if (context.GunData.shells)
            {
                Debug.Log("I know its Shotgun");
                HandleShotgunShot(context);
                return;
            }

            HandleSingleShot(context);
        }
        
        private void HandleSingleShot(WeaponContext context)
        {
            bool hit = Physics.Raycast(
                context.Origin.position,
                context.Direction ?? context.Origin.forward,
                out RaycastHit hitInfo,
                context.GunData.lengthRange
            );

            if (hit)
            {
                _damageHandler.HandleDamage(new DamageContext
                {
                    HitInfo = hitInfo,
                    GunData = context.GunData,
                    AutoAimAllowed = context.AutoAimAllowed,
                    Origin = context.Origin
                });
            }
        }
        
        private void HandleShotgunShot(WeaponContext context)
        {
            Debug.Log("HandleShotgunShot");
            var hits = CreateShotgunSpread(context);
            _damageHandler.HandleDamage(new DamageContext
            {
                MultipleHits = hits,
                GunData = context.GunData,
                AutoAimAllowed = context.AutoAimAllowed,
                Origin = context.Origin
            });
        }
        
        private List<RaycastHit> CreateShotgunSpread(WeaponContext context)
        {
            Debug.Log("SPREADING");
            Vector3[] offsets =
            {
                new Vector3(0f, 0f, 0f),
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
                Vector3 spreadDirection = context.Origin.forward
                                          + context.Origin.right * offset.x
                                          + context.Origin.up * offset.y;
                spreadDirection.Normalize();

                if (Physics.Raycast(context.Origin.position, spreadDirection, out RaycastHit hitInfo,
                        context.GunData.lengthRange))
                {
                    raycastHits.Add(hitInfo);
                }
            }

            return raycastHits;
        }
        
        // public static void ShootSingleRay(Transform origin, GunData gunData, bool autoAimAllowed, Vector3? direction = null)
        // {
        //     RaycastHit hitInfo;
        //     bool hitTarget;
        //     if (direction != null)
        //     {
        //         hitTarget = Physics.Raycast(origin.position, (Vector3)direction, out hitInfo, gunData.lengthRange);
        //         if (hitTarget) DamageSystem.Instance.HandleHit(hitInfo, autoAimAllowed, gunData, origin);
        //         return;
        //     }
        //     
        //     hitTarget = Physics.Raycast(origin.position, origin.forward, out hitInfo, gunData.lengthRange);
        //     if (hitTarget) DamageSystem.Instance.HandleHit(hitInfo, autoAimAllowed, gunData, origin);
        // }
        //
        // public static void ShootSingleRayWithAutoAim(Transform _origin, GunData gunData, GameObject enemy)
        // {
        //     Vector3 origin = _origin.position;
        //     Vector3 direction = (enemy.transform.position - origin).normalized;
        //     
        //     ShootSingleRay(_origin, gunData, false, direction);
        // }
        //
        // private static void ShootShotgun(Transform origin, GunData gunData, bool autoAimAllowed, Vector3? direction = null)
        // {
        //     Vector3[] offsets =
        //     {
        //         Vector3.zero,
        //         new Vector3(0.01f, 0f, 0f),
        //         new Vector3(-0.01f, 0f, 0f),
        //         new Vector3(0.02f, 0f, 0f),
        //         new Vector3(-0.02f, 0f, 0f),
        //         new Vector3(0.03f, 0f, 0f),
        //         new Vector3(-0.03f, 0f, 0f)
        //     };
        //
        //     List<RaycastHit> raycastHits = new List<RaycastHit>();
        //
        //     foreach (var offset in offsets)
        //     {
        //         Vector3 spreadDirection = origin.forward
        //                                   + origin.right * offset.x
        //                                   + origin.up * offset.y;
        //         spreadDirection.Normalize();
        //
        //         if (Physics.Raycast(origin.position, spreadDirection, out RaycastHit hitInfo,
        //                 gunData.lengthRange))
        //         {
        //             raycastHits.Add(hitInfo);
        //         }
        //     }
        //
        //     DamageSystem.Instance.HandleHit(raycastHits, false, gunData, origin);
        // }
        //
        // public static void ShootShotGunWithAutoAim(Transform _origin, GunData gunData, GameObject enemy)
        // {
        //     Vector3 origin = _origin.position;
        //     Vector3 direction = (enemy.transform.position - origin).normalized;
        //     
        //     ShootShotgun(_origin, gunData, false, direction);
        // }
    }
}