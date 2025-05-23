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
        
    }
}