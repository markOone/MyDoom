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
            if (ArmorBonus) PlayerStats.Instance.ArmorTaking(1);
            if(Armor) PlayerStats.Instance.ArmorTaking(100);
            if(MegaArmor) PlayerStats.Instance.ArmorTaking(200);
            
            if(HealthBonus) PlayerStats.Instance.HealthKit(1);
            if(Stimpack) PlayerStats.Instance.HealthKit(10);
            if(MedKit) PlayerStats.Instance.HealthKit(25);
            if(Supercharge) PlayerStats.Instance.HealthKit(100);
                
            Invoke("DestroyObject", 0.1f);
        }
    
        void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}

