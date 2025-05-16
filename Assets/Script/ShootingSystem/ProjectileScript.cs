using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDoom.ShootingSystem
{
    public class ProjectileScript : MonoBehaviour
    {
        void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Enemy"))
            {
                Invoke("Die", .1f);
                gameObject.GetComponent<Collider>().enabled = false;
            }

            if (other.transform.gameObject.CompareTag("Player"))
            {
                PlayerStats.Instance.TakeDamage(5);
                //Invoke("Die", 1f);
            }

            //if(other.transform.gameObject.CompareTag("Wall")) Invoke("Die", .1f);
        }

        private void Start()
        {
            //Invoke("Die", 2f);
        }

        void Die()
        {
            Destroy(this.gameObject);
        }
    }
}