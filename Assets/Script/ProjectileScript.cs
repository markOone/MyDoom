using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.gameObject.CompareTag("Player"))
        {
            PlayerStats.Instance.TakeDamage(5);
            Invoke("Die", 1f);
        }
        
        if(other.transform.gameObject.CompareTag("Wall")) Invoke("Die", 1f);
    }

    private void Start()
    {
        Invoke("Die", 2f);
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
