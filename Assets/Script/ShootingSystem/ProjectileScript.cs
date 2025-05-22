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
            if(other.gameObject.layer.ToString() == "FireBall") return;
            GameObject explotion = Instantiate(effectPrefab, transform.position, Quaternion.identity);
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
            foreach (Collider collider in colliders)
            {
                IDamagable damagable = collider.gameObject.GetComponent<IDamagable>();
                if(damagable != null) damagable.Damage(Damage, 100f);
            }
            gameObject.GetComponent<Collider>().enabled = false;
            Invoke("Die", .1f);
        }

        void Die()
        {
            Destroy(this.gameObject);
        }
    }
}