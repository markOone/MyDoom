using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace MyDoom.Enemies
{
    public class Enemy : MonoBehaviour, IDamagable
    {
        [SerializeField] float health = 100;
        private Transform playerPosition;
        public LayerMask playerMask;
        [SerializeField] NavMeshAgent agent;
        [SerializeField] AudioSource audioSourcePain;
        [SerializeField] AudioSource audioSourceDeath;
        [SerializeField] Transform projectileSpawnPoint;

        [Header("For Patrolling")] [SerializeField]
        private Vector3 walkPoint;

        [SerializeField] bool walkPointSet;
        public LayerMask whatIsGround;
        public static float walkPointRange;

        [Header("For Attacking")] public float timeBetweenAttacks;
        bool alreadyAttacked;
        [SerializeField] GameObject projectile;
        [SerializeField] Transform projectileSpawnPoint;

        [Header("States")] public float sightRange, attackRange;
        public bool playerInSightRange, playerInAttackRange;

        //Memo
        private Func<float, float, float> _memoizedCalculateDamage;

        private void Start()
        {
            playerPosition = GameManager.Instance.playerPosition;
            _memoizedCalculateDamage = new Func<float, float, float>(CalculateDamage).Memoize(
                new Memoizer.MemoizationOptions
                {
                    Size = 50,
                    Policy = Memoizer.EvictionPolicy.LRU
                });
        }

        public void Damage(float damage, float distance)
        {
            damage = _memoizedCalculateDamage(damage, distance) * Random.Range(0, 4);
            health -= damage;
            audioSourcePain?.Play(0);
            //Debug.Log(health);
            if (health <= 0)
            {
                audioSourceDeath?.Play(0);
                Destroy(gameObject);
            }
        }

        public float CalculateDamage(float damage, float distance)
        {
            float result = damage * (1 - 1 / distance);

            return result;
        }

        private void Update()
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

            if (!playerInSightRange && !playerInAttackRange) Patrol();
            if (playerInSightRange && !playerInAttackRange) Chase();
            if (playerInAttackRange && playerInSightRange) Attack();
        }

        void CheckIfNotInTheWall()
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f))
            {
                if (hit.collider.gameObject.CompareTag("Wall") || hit.collider.gameObject.CompareTag("Enemy"))
                {
                    //Debug.Log("In the wall");
                    walkPointSet = false;
                }
            }
        }

        void Patrol()
        {
            //Debug.Log("Patrolling");
            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet) agent.SetDestination(walkPoint);
            CheckIfNotInTheWall();

            Vector3 distanceToWalkPoint = walkPoint - transform.position;

            if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
        }

        void Chase()
        {
            //Debug.Log("Chasing");
            agent.isStopped = false;
            agent.SetDestination(playerPosition.position);
        }

        void Attack()
        {
            //Debug.Log("Attacking");
            if (CheckForObstacle(playerPosition.position))
            {
                Chase();
                return;
            }
            else
            {
                agent.SetDestination(gameObject.transform.position);
                agent.isStopped = true;
                agent.ResetPath();
            }

            transform.LookAt(playerPosition);
            if (!alreadyAttacked)
            {
                Rigidbody rb = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity)
                    .GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 64f, ForceMode.Impulse);
                rb.AddForce(transform.up * 4f, ForceMode.Impulse);

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }

        bool CheckForObstacle(Vector3 playerPosition)
        {
            Vector3 directionToPlayer = (playerPosition - transform.position).normalized;
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, attackRange + 5f))
            {
                if (hit.collider.gameObject.CompareTag("Wall") || hit.collider.gameObject.CompareTag("Enemy"))
                {
                    //Debug.Log("In the wall");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        void ResetAttack()
        {
            alreadyAttacked = false;
        }

        void SearchWalkPoint()
        {
            //Debug.Log("Searching for a walk point");
            float randomZ = UnityEngine.Random.Range(-10f, 10f);
            float randomX = UnityEngine.Random.Range(-10f, 10f);

            walkPoint = new Vector3(transform.position.x + randomX, gameObject.transform.position.y,
                transform.position.z + randomZ);

            if (Physics.Raycast(walkPoint, -transform.up, out RaycastHit hit, gameObject.transform.localScale.y + 2f,
                    whatIsGround))
            {
                walkPointSet = true;
            }
            else
            {
                //Debug.Log("NO GROUND HEEEEELP");
                Debug.DrawRay(hit.point, -transform.up, Color.blue, 2f);
            }
        }
    }
}


