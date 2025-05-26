using System.Collections.Generic;
using UnityEngine;

namespace MyDoom.ShootingSystem
{
    public class WeaponContext
    {
        public Transform Origin { get; set; }
        public GunData GunData { get; set; }
        public bool AutoAimAllowed { get; set; }
        public Vector3? Direction { get; set; }
        public GameObject ProjectilePrefab { get; set; } // Only for projectile weapons
        public float ProjectileForce { get; set; } = 300f; // Default force for projectiles
        public bool isHand { get; set; } = true;
    }

    public class DamageContext
    {
        public RaycastHit HitInfo { get; set; }
        public List<RaycastHit> MultipleHits { get; set; }
        public GunData GunData { get; set; }
        public bool AutoAimAllowed { get; set; }
        public Transform Origin { get; set; }
    }

    public class AutoAimContext
    {
        public Transform Origin { get; set; }
        public GunData GunData { get; set; }
        public float FieldOfView { get; set; }
        public int RayCount { get; set; }
    }
}