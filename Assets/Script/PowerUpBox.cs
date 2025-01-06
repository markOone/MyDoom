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
            if (ArmorBonus) PlayerStats.Instance.ArmorBonus();
            if(Armor) PlayerStats.Instance.ArmorKit();
            if(MegaArmor) PlayerStats.Instance.MegaArmor();
            
            if(HealthBonus) PlayerStats.Instance.HealthBonus();
            if(Stimpack) PlayerStats.Instance.Stimpack();
            if(MedKit) PlayerStats.Instance.MedKit();
            if(Supercharge) PlayerStats.Instance.Supercharge();
                
            Invoke("DestroyObject", 0.1f);
        }
    }
    
    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
