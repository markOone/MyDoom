using UnityEngine;
using MyDoom.ShootingSystem;

namespace MyDoom.Enemies
{
    public class ZombieShotgun : Enemy
    {
        IWeaponSystem hitScanShootingSystem;
        [SerializeField] private GunData gunData;
        int attackCounter = 0;
        
        private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
        
        protected override void Attack()
        {
            hitScanShootingSystem = HitScanShooting.Instance;
            if (base.CheckForObstacle(base.playerPosition.position))
            {
                base.Chase();
                return;
            }
            else
            {
                base.agent.SetDestination(gameObject.transform.position);
                base.agent.isStopped = true;
                base.agent.speed = 0f;
                base.agent.ResetPath();
            }

            transform.LookAt(playerPosition);
            if (CanShoot())
            {
                hitScanShootingSystem.Shoot(new WeaponContext()
                {
                    Origin = projectileSpawnPoint,
                    Direction = playerPosition.position - projectileSpawnPoint.position,
                    GunData = gunData,
                    AutoAimAllowed = false
                });
                attackCounter++;
                Debug.Log(attackCounter);
                timeSinceLastShot = 0f;
            }
        }
    }
}