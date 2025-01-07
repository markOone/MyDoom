using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwarness : MonoBehaviour
{
    [SerializeField] internal bool isAggressive;
    Collider[] colliders;
    [SerializeField] LayerMask playerMask;
    

    private void Awake()
    {
        isAggressive = false;
    }

    private void FixedUpdate()
    {
        CheckPlayer();
    }


    public void CheckPlayer()
    {
        colliders = Physics.OverlapSphere(transform.position, 50f, playerMask);
        foreach (var enemy in colliders)
        {
            if (enemy.CompareTag("Player"))
            {
                isAggressive = true;
            }
            else
            {
                isAggressive = false;
            }
        }
    }
}
