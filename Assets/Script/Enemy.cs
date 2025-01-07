using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] int health = 100;
    private Transform playerPosition;
    public LayerMask playerMask;
    [SerializeField] NavMeshAgent agent;

    [Header("For Patrolling")] 
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] bool walkPointSet;
    public LayerMask whatIsGround;
    public static float walkPointRange;
    
    [Header("For Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    [SerializeField] GameObject projectile;

    [Header("States")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Start()
    { 
        playerPosition = GameManager.Instance.playerPosition;
    }

    public void Damage(int damage)
    {
        health -= damage;
        
        if (health <= 0) Destroy(gameObject);
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        // if (!playerInSightRange && !playerInAttackRange) Patrol();
        if (playerInSightRange && !playerInAttackRange) Chase();
        if (playerInAttackRange && playerInSightRange) Attack();
    }

    void CheckIfNotInTheWall()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f))
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                walkPointSet = false;
            }
        }
    }

    // void Patrol()
    // {
    //     Debug.Log("Patrolling");
    //     if (!walkPointSet) SearchWalkPoint();
    //     
    //     if(walkPointSet) agent.SetDestination(walkPoint);
    //     CheckIfNotInTheWall();
    //     
    //     Vector3 distanceToWalkPoint = walkPoint - transform.position;
    //     
    //     if(distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    // }

    void Chase()
    {
        Debug.Log("Chasing");
        agent.SetDestination(playerPosition.position);
    }

    void Attack()
    {
        Debug.Log("Attacking");
        agent.SetDestination(gameObject.transform.position);
        
        transform.LookAt(playerPosition);
        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 64f, ForceMode.Impulse);
            rb.AddForce(transform.up * 4f, ForceMode.Impulse);
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // void SearchWalkPoint(){
    //     Debug.Log("Searching for a walk point");
    //     float randomZ = UnityEngine.Random.Range(-10f, 10f);
    //     float randomX = UnityEngine.Random.Range(-10f, 10f);
    //     
    //     walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    //     
    //     if (Physics.Raycast(walkPoint, -transform.up, out RaycastHit hit, 2f, whatIsGround))
    //     {
    //         walkPointSet = true;
    //     }
    // }
}
