using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyDoom.ShootingSystem;

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
            if (Clip) PlayerStats.Instance.TakeAmmo(10, AmmoType.Bullet);
            if (BoxOfBullets) PlayerStats.Instance.TakeAmmo(50 , AmmoType.Bullet);
            if (FShells) PlayerStats.Instance.TakeAmmo(4, AmmoType.Shell);
            if (BoxOfShells) PlayerStats.Instance.TakeAmmo(20, AmmoType.Shell);
            if (Rocket) PlayerStats.Instance.TakeAmmo(1, AmmoType.Rocket);  
            if (BoxOfRockets) PlayerStats.Instance.TakeAmmo(10, AmmoType.Rocket);
            if (Cell) PlayerStats.Instance.TakeAmmo(20, AmmoType.Cell);
            if (CellPack) PlayerStats.Instance.TakeAmmo(100, AmmoType.Cell);
            
            Invoke("DestroyObject", 0.1f);
        }

        void DestroyObject()
        {
            Destroy(gameObject);
        }


    }
}

