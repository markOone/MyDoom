using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingField : MonoBehaviour
{
    [SerializeField] public List<GameObject> EnemiesInField = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")) EnemiesInField.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy")) EnemiesInField.Remove(other.gameObject);
    }

    public void ChangeRange(float wideRange, float legthRange)
    {
        this.gameObject.transform.localScale = new Vector3(wideRange, 5.4f, legthRange);
    }
}
