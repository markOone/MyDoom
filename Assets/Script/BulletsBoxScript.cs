using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsBoxScript : MonoBehaviour
{
    [SerializeField] private int BulletsAmount;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerStats.Instance.TakeBullets(BulletsAmount);
        }
    }
}
