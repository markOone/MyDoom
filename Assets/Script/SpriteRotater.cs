using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDoom.GeneralSystems;

public class SpriteRotater : MonoBehaviour
{
    void Update()
    {
        RotateTowardsPlayer(GameManager.Instance.playerPosition.position);
    }

    void RotateTowardsPlayer(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
    }
}
