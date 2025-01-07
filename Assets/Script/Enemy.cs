using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] int health = 100;
    [SerializeField] EnemyAwarness enemyAwarness;
    private Transform playerPosition;
    [SerializeField] NavMeshAgent agent;

    private void Start()
    { 
        playerPosition = GameManager.Instance.playerPosition;
    }

    public void Damage(int damage)
    {
        health -= damage;
        
        if (health <= 0) Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (enemyAwarness.isAggressive)
        {
            agent.SetDestination(playerPosition.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }
}
