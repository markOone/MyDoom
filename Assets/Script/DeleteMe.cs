using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMe : MonoBehaviour
{
    [SerializeField] float timeToKill;

    void Start()
    {
        Die();
    }
    
    void Die() => Destroy(gameObject, timeToKill);
}
