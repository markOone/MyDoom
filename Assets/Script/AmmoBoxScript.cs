using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxScript : MonoBehaviour
{
    [SerializeField] int AmmoAmount;
    [SerializeField] bool Bullets, Shells, Rockets, Cells;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Bullets) PlayerStats.Instance.TakeBullets(AmmoAmount);
            if(Shells) PlayerStats.Instance.TakeShells(AmmoAmount);
            if(Rockets) PlayerStats.Instance.TakeRockets(AmmoAmount);
            if(Cells) PlayerStats.Instance.TakeCells(AmmoAmount);
            Invoke("DestroyObject", 0.1f);
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
