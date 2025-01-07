using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBox : MonoBehaviour
{
    [SerializeField] bool ArmorBonus, Armor, MegaArmor;
    [SerializeField] bool HealthBonus, Stimpack, MedKit, Supercharge;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (ArmorBonus) PlayerStats.Instance.ArmorTaking(1);
            if(Armor) PlayerStats.Instance.ArmorTaking(2);
            if(MegaArmor) PlayerStats.Instance.ArmorTaking(3);
            
            if(HealthBonus) PlayerStats.Instance.HealthKit(1);
            if(Stimpack) PlayerStats.Instance.HealthKit(2);
            if(MedKit) PlayerStats.Instance.HealthKit(3);
            if(Supercharge) PlayerStats.Instance.HealthKit(4);
                
            Invoke("DestroyObject", 0.1f);
        }
    }
    
    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
