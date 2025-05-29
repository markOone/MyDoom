using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MyDoom.ShootingSystem
{
    public class ProjectileScript : MonoBehaviour
    {
        private IObjectPool<GameObject> objectPool;
        public float Damage = 10f;
        [SerializeField] GameObject effectPrefab;
        private bool isReleased = false;
        
        public IObjectPool<GameObject> ObjectPool
        {
            get => objectPool;
            set => objectPool = value;
        }
        
        void OnEnable()
        {
            isReleased = false;
            GetComponent<Collider>().enabled = true;
        }

        void OnCollisionEnter(Collision other)
        {
            if (isReleased) return;
            
            Debug.Log("Collision with: " + other.gameObject.name);
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
            Invoke(nameof(Deactivate), .1f);
        }
        
        public void Deactivate()
        {
            if (isReleased) return;
            isReleased = true;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0f, 0f, 0f);
            rb.angularVelocity = new Vector3(0f, 0f, 0f);

            objectPool.Release(gameObject);
        }
        
        void Die()
        {
            Destroy(this.gameObject);
        }
    }
}