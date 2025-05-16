using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDoom.InGameObjects
{
    public class AmmoBoxScript : MonoBehaviour, IInteractable
    {
        [SerializeField] bool Clip, BoxOfBullets;
        [SerializeField] bool FShells, BoxOfShells;
        [SerializeField] bool Rocket, BoxOfRockets;
        [SerializeField] bool Cell, CellPack;
    
        public void Interact()
        {
            if (Clip) PlayerStats.Instance.TakingAmmo(10, AmmoType.Bullet);
            if (BoxOfBullets) PlayerStats.Instance.TakingAmmo(50 , AmmoType.Bullet);
            if (FShells) PlayerStats.Instance.TakingAmmo(4, AmmoType.Shell);
            if (BoxOfShells) PlayerStats.Instance.TakingAmmo(20, AmmoType.Shell);
            if (Rocket) PlayerStats.Instance.TakingAmmo(1, AmmoType.Rocket);  
            if (BoxOfRockets) PlayerStats.Instance.TakingAmmo(10, AmmoType.Rocket);
            if (Cell) PlayerStats.Instance.TakingAmmo(20, AmmoType.Cell);
            if (CellPack) PlayerStats.Instance.TakingAmmo(100, AmmoType.Cell);
            
            Invoke("DestroyObject", 0.1f);
        }

        void DestroyObject()
        {
            Destroy(gameObject);
        }


    }
}

