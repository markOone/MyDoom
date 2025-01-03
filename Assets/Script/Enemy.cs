using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] int health = 100;

    public void Damage(int damage)
    {
        health -= damage;
        
        if (health <= 0) Destroy(gameObject);
    }
}
