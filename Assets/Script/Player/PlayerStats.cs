using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Object")]
    private static PlayerStats _instance;
    
    [Header("References")]
    [SerializeField] Animator damageAnimator;
    [SerializeField] AudioSource audioSource;
    
    [Header("Player Stats")]
    int Health;
    int MaxHealth = 100;

    int Armor;
    int MaxArmor = 200;
    

    public int BulletsCounter = 0;
    public int ShellCounter = 0;
    public int RocketsCounter = 0;
    public int CellsCounter = 0;

    public static PlayerStats Instance { get { if (_instance == null) Debug.Log("No PlayerStats"); return _instance; } }
    
    private void OnEnable()
    {
        _instance = this;
    }

    void Awake()
    {
        _instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Health = MaxHealth;
        Armor = 0;
        HudController.Instance.UpdateHUD();
    }

    private void FixedUpdate()
    {
        if(Health <= 0) Invoke("RestartingGame", 2f);
    }

    public void RestartingGame() => GameManager.Instance.RestartGame();

    public void TakeDamage(int damage)
    {
        audioSource?.Play(0);
        if (Armor > 0)
        {
            if (Armor >= damage) Armor -= damage;
            else
            {
                int remainingDamage = damage - Armor; 
                Health -= remainingDamage;
                Armor = 0;
            }
        }
        else
        {
            Health -= damage;
        }
        
        damageAnimator.SetTrigger("Damage");
        HudController.Instance.UpdateHUD();
        if (Health <= 0)
        {
            Invoke("RestartingGame", 2f);
        }
    }

    public void ArmorTaking(int type)
    {
        GameManager.Instance.powerUpEffect();
        
        if (type == 1)
        {
            Armor += 1;
            if(Armor > MaxArmor) Armor = MaxArmor;
        }

        if (type == 2)
        {
            if(Armor < 100) Armor = 100;
        }

        if (type == 3)
        {
            if(Armor < MaxArmor) Armor = MaxArmor;
        }
        
        
        HudController.Instance.UpdateHUD();
    }
    
    public void HealthKit(int type)
    {
        GameManager.Instance.powerUpEffect();
        
        if(type == 1)
        {
            Health += 1;
            if(Health > 200) Health = 200;
        }

        if (type == 2)
        {
            Health += 10;
            if(Health > 100) Health = 100;
        }

        if (type == 3)
        {
            Health += 25;
            if(Health > 100) Health = 100;
        }

        if (type == 4)
        {
            Health += 100;
            if(Health > 200) Health = 200;
        }
        
        HudController.Instance.UpdateHUD();
    }

    public void TakingAmmo(int type, int ammoType)
    {
        GameManager.Instance.powerUpEffect();
        
        if (ammoType == 1)
        {
            if (type == 1)
            {
                BulletsCounter += 10;
                if(BulletsCounter > 200) BulletsCounter = 200;
                
            }

            if (type == 2)
            {
                BulletsCounter += 50;
                if(BulletsCounter > 200) BulletsCounter = 200;
                
            }
            
            if(PlayerShooting.Instance.currentGunData.bullets) PlayerShooting.Instance.currentGunData.currentAmmo = BulletsCounter;
        }

        if (ammoType == 2)
        {
            if (type == 1)
            {
                ShellCounter += 4;
                if(ShellCounter > 100) ShellCounter = 100;
            }

            if (type == 2)
            {
                ShellCounter += 20;
                if(ShellCounter > 100) ShellCounter = 100;
            }
            
            if(PlayerShooting.Instance.currentGunData.shells) PlayerShooting.Instance.currentGunData.currentAmmo = ShellCounter;
        }

        if (ammoType == 3)
        {
            if (type == 1)
            {
                RocketsCounter += 1;
                if(RocketsCounter > 100) RocketsCounter = 100;
            }

            if (type == 2)
            {
                RocketsCounter += 10;
                if(RocketsCounter > 100) RocketsCounter = 100;
            }
            
            if(PlayerShooting.Instance.currentGunData.rockets) PlayerShooting.Instance.currentGunData.currentAmmo = RocketsCounter;
        }

        if (ammoType == 4)
        {
            if (type == 1)
            {
                CellsCounter += 20;
                if(CellsCounter > 300) CellsCounter = 300;
            }

            if (type == 2)
            {
                CellsCounter += 100;
                if(CellsCounter > 300) CellsCounter = 300;
            }
            
            if(PlayerShooting.Instance.currentGunData.cells) PlayerShooting.Instance.currentGunData.currentAmmo = CellsCounter;
        }
        
        HudController.Instance.UpdateHUD();
    }
    
    public int GetHealth() => Health;
    public int GetBullets() => BulletsCounter;
    public int GetShells() => ShellCounter;
    public int GetRockets() => RocketsCounter;
    public int GetArmor() => Armor;
    public int GetCells() => CellsCounter;

    // public void GetStatsFromGunData(Transform[] weaponsList)
    // {
    //     for (int i = 0; i < weaponsList.Length; i++)
    //     {
    //         if(weaponsList[i].gameObject.GetComponent<Gun>().gunData.shells) ShellCounter += weaponsList[i].gameObject.GetComponent<Gun>().gunData.currentAmmo;
    //         if (weaponsList[i].gameObject.GetComponent<Gun>().gunData.bullets) BulletsCounter += weaponsList[i].gameObject.GetComponent<Gun>().gunData.currentAmmo;
    //         if(weaponsList[i].gameObject.GetComponent<Gun>().gunData.rockets) RocketsCounter += weaponsList[i].gameObject.GetComponent<Gun>().gunData.currentAmmo;
    //         if(weaponsList[i].gameObject.GetComponent<Gun>().gunData.cells) CellsCounter += weaponsList[i].gameObject.GetComponent<Gun>().gunData.currentAmmo;
    //     }
    //     HudController.Instance.UpdateHUD();
    // }
    
    
}
