using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxScript : MonoBehaviour
{
    [SerializeField] bool Clip, BoxOfBullets;
    [SerializeField] bool FShells, BoxOfShells;
    [SerializeField] bool Rocket, BoxOfRockets;
    [SerializeField] bool Cell, CellPack;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Clip) PlayerStats.Instance.TakingAmmo(1, 1);
            if (BoxOfBullets) PlayerStats.Instance.TakingAmmo(2, 1);
            if (FShells) PlayerStats.Instance.TakingAmmo(1, 2);
            if (BoxOfShells) PlayerStats.Instance.TakingAmmo(2, 2);
            if (Rocket) PlayerStats.Instance.TakingAmmo(1, 3);  
            if (BoxOfRockets) PlayerStats.Instance.TakingAmmo(2, 3);
            if (Cell) PlayerStats.Instance.TakingAmmo(1, 4);
            if (CellPack) PlayerStats.Instance.TakingAmmo(2, 4);
            
            Invoke("DestroyObject", 0.1f);
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
