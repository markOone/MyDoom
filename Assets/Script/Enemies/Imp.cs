using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDoom.ShootingSystem;
using UnityEngine.PlayerLoop;

namespace MyDoom.Enemies
{
    public class Imp : Enemy
    {
        IWeaponSystem projectileShootingSystem;
        IWeaponSystem hitScanShootingSystem;
        [SerializeField] private GunData gunData;
        [SerializeField] private GunData handHeldGunData;
        int attackCounter = 0;
        
        private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
        
        protected override void Attack()
        {
            projectileShootingSystem = ProjectileShooting.Instance;
            hitScanShootingSystem = HitScanShooting.Instance;
            if (base.CheckForObstacle(base.playerPosition.position))
            {
                base.Chase();
                return;
            }

            base.agent.SetDestination(gameObject.transform.position);
            base.agent.isStopped = true;
            base.agent.speed = 0f;
            base.agent.ResetPath();
            

            transform.LookAt(playerPosition);
            if (CheckDistance(playerPosition.position) <= 5f)
            {
                if (CanShoot())
                {
                    hitScanShootingSystem.Shoot(new WeaponContext()
                    {
                        Origin = projectileSpawnPoint,
                        Direction = playerPosition.position - projectileSpawnPoint.position,
                        GunData = handHeldGunData,
                        AutoAimAllowed = false
                    });
                    timeSinceLastShot = 0f;
                    return;
                }
            }
            
            if (CanShoot())
            {
                projectileShootingSystem.Shoot(new WeaponContext()
                {
                    Origin = projectileSpawnPoint,
                    //Direction = playerPosition.position - projectileSpawnPoint.position,
                    GunData = gunData,
                    AutoAimAllowed = false,
                    ProjectilePrefab = projectilePrefab,
                    ProjectileForce = 60f
                });
                attackCounter++;
                timeSinceLastShot = 0f;
            }
        }
    }
}

