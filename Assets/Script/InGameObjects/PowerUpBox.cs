using UnityEngine;
using MyDoom.InGameObjects;


namespace MyDoom.ShootingSystem
{
    public class PowerUpBox : MonoBehaviour, IInteractable
    {
        [SerializeField] bool ArmorBonus, Armor, MegaArmor;
        [SerializeField] bool HealthBonus, Stimpack, MedKit, Supercharge;
    
        public void Interact()
        {
            if (ArmorBonus) PlayerStats.Instance.TakeArmor(1);
            if(Armor) PlayerStats.Instance.TakeArmor(100);
            if(MegaArmor) PlayerStats.Instance.TakeArmor(200);
            
            if(HealthBonus) PlayerStats.Instance.TakeHealthKit(1);
            if(Stimpack) PlayerStats.Instance.TakeHealthKit(10);
            if(MedKit) PlayerStats.Instance.TakeHealthKit(25);
            if(Supercharge) PlayerStats.Instance.TakeHealthKit(100);
                
            Invoke("DestroyObject", 0.1f);
        }
    
        void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}

