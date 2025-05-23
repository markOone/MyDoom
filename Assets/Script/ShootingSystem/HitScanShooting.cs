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
            
            Vector3 baseForward = context.Direction ?? context.Origin.forward;
            Quaternion lookRotation = Quaternion.LookRotation(baseForward);
            Vector3 spreadRight = lookRotation * Vector3.right;
            Vector3 spreadUp = lookRotation * Vector3.up;

        
            foreach (var offset in offsets)
            {
                Vector3 spreadDirection = baseForward
                                          + spreadRight * offset.x
                                          + spreadUp * offset.y;
                spreadDirection.Normalize();
        
                if (Physics.Raycast(context.Origin.position, spreadDirection, out RaycastHit hitInfo,
                        context.GunData.lengthRange))
                {
                    raycastHits.Add(hitInfo);
                }
            }
        
            return raycastHits;
        }
    }
}