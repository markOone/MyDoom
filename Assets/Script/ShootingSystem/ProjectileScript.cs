using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDoom.ShootingSystem
{
    public class ProjectileScript : MonoBehaviour
    {
        public float Damage = 10f;
        [SerializeField] GameObject effectPrefab;
        void OnCollisionEnter(Collision other)
        {
            Collider[] colliders = new Collider[10];
            if(other.gameObject.layer.ToString() == "FireBall") return;
            GameObject explotion = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            Physics.OverlapSphereNonAlloc(transform.position, 5f, colliders);
            
            for(int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] == null) continue;
                IDamagable? damageable = colliders[i].gameObject.GetComponent<IDamagable>();
                if(damageable != null) damageable.Damage(Damage, 100f);
            }
            
            gameObject.GetComponent<Collider>().enabled = false;
            Invoke(nameof(Die), .1f);
        }

        void Die()
        {
            Destroy(this.gameObject);
        }
    }
}