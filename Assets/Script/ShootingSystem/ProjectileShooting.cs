using MyDoom.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace MyDoom.ShootingSystem
{
    public class ProjectileShooting : MonoBehaviour, IWeaponSystem
    {
        [SerializeField] private float projectileForce = 300f;
        private IAutoAimSystem _autoAimSystem;
        
        private static ProjectileShooting _instance;
        
        public static ProjectileShooting Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log("No ProjectileShooting");
                }

                return _instance;
            }
        }
        
        void OnEnable()
        {
            _instance = this;
        }
        
        private void Awake()
        {
            _autoAimSystem = GetComponent<IAutoAimSystem>();
        }
        
        public void Shoot(WeaponContext context)
        {
            if (!context.ProjectilePrefab)
            {
                Debug.LogError("Projectile prefab is missing!");
                return;
            }

            Vector3 direction = context.Direction ?? context.Origin.forward;
            
            if (context.AutoAimAllowed)
            {
                var target = _autoAimSystem.ChooseEnemyWithAutoAim(new AutoAimContext
                {
                    Origin = context.Origin,
                    GunData = context.GunData,
                    FieldOfView = 60f,
                    RayCount = 15
                });

                if (target != null)
                {
                    direction = (target.transform.position - context.Origin.position).normalized;
                }
            }

            LaunchProjectile(context, direction);
        }
        
        private void LaunchProjectile(WeaponContext context, Vector3 direction)
        {
            var projectile = Instantiate(
                context.ProjectilePrefab,
                context.Origin.position,
                context.Origin.rotation
            );

            var rb = projectile.GetComponent<Rigidbody>();
            var projectileComponent = projectile.GetComponent<ProjectileScript>();

            if (projectileComponent != null)
            {
                projectileComponent.Damage = context.GunData.damage;
            }

            if (rb != null)
            {
                rb.AddForce(direction * projectileForce, ForceMode.Impulse);
            }
        }
        
        // public void ShootProjectile(Transform origin, GameObject projectilePrefab, GunData gunData, Vector3? direction = null)
        // {
        //     bool hitTarget = Physics.Raycast(origin.position, origin.forward, out RaycastHit hitInfo, gunData.lengthRange);
        //     
        //     HandleParticleShot(hitTarget, hitInfo);
        //     
        //     Vector3 spawnPoint = origin.position;
        //     Rigidbody rb = Instantiate(projectilePrefab, spawnPoint, origin.rotation).GetComponent<Rigidbody>();
        //     rb.gameObject.GetComponent<ProjectileScript>().Damage = gunData.damage;
        //     if (direction != null)
        //     {
        //         rb.AddForce((Vector3)(direction * 300f), ForceMode.Impulse);
        //     }
        //     else
        //     {
        //         rb.AddForce(origin.forward * 300f, ForceMode.Impulse);
        //     }
        // }
        //
        // public void ShootPartickeWithAutoAim(Transform _origin, GameObject enemy, GunData gunData, GameObject projectilePrefab)
        // {
        //     //Debug.Log("ShootPartickeWithAutoAim");
        //     Vector3 origin = _origin.position;
        //     Vector3 direction = (enemy.transform.position - origin).normalized;
        //
        //     PlayerShooting.Instance.muzzleFlashEffect.Play();
        //     ShootProjectile(_origin, projectilePrefab, gunData,direction);
        // }
        //
        // void HandleParticleShot(bool hitTarget, RaycastHit hitInfo)
        // {
        //     if (!hitTarget)
        //     {
        //         ProjectileShooting.Instance.ShootProjectile();
        //         return;
        //     }
        //
        //     IDamagable? damagable = hitInfo.collider.GetComponent<IDamagable>();
        //     if (damagable != null)
        //     {
        //         ShootParticle();
        //         return;
        //     }
        //
        //     GameObject targetEnemy = ChooseEnemyWithAutoAim();
        //     if (targetEnemy != null)
        //     {
        //         ShootPartickeWithAutoAim(targetEnemy);
        //     }
        //     else
        //     {
        //         ShootParticle();
        //     }
        // }
    }
}