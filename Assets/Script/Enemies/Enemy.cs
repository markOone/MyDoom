using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using MyDoom.ShootingSystem;

namespace MyDoom.Enemies
{
    public class Enemy : MonoBehaviour, IDamagable
    {
        [SerializeField] float health = 100;
        [SerializeField] float speed = 12f;
        internal Transform playerPosition;
        public LayerMask playerMask;
        [SerializeField] protected NavMeshAgent agent;
        [SerializeField] AudioSource audioSourcePain;
        [SerializeField] AudioSource audioSourceDeath;
        [SerializeField] protected Transform projectileSpawnPoint;

        [Header("For Patrolling")] [SerializeField]
        private Vector3 walkPoint;

        [SerializeField] bool walkPointSet;
        public LayerMask whatIsGround;
        public static float walkPointRange;

        [Header("For Attacking")]
        protected float timeSinceLastShot;
        [SerializeField] protected GameObject projectile;

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
            
            timeSinceLastShot += Time.deltaTime;
            
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
            agent.isStopped = false;
            agent.speed = speed / 2f;
            CheckIfNotInTheWall();

            Vector3 distanceToWalkPoint = walkPoint - transform.position;

            if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
        }

        protected void Chase()
        {
            //Debug.Log("Chasing");
            agent.isStopped = false;
            agent.SetDestination(playerPosition.position);
            agent.speed = speed;
        }

        protected virtual void Attack()
        {
            Debug.Log("Attacking");
        }

        protected bool CheckForObstacle(Vector3 playerPosition)
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


