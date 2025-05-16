using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using MyDoom.Player;

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

    public void ArmorTaking(int amount)
    {
        GameManager.Instance.powerUpEffect();

        if (amount == 100)
        {
            Armor += amount;
            if(Armor < 100) Armor = 100;
        }
        else
        {
            Armor += amount;
        }
        
        if(Armor > MaxArmor) Armor = MaxArmor;
        
        HudController.Instance.UpdateHUD();
    }
    
    public void HealthKit(int amount)
    {
        GameManager.Instance.powerUpEffect();
        
        if (amount is 10 or 25)
        {
            Health += amount;
            if(Health < 100) Armor = 100;
        }
        else
        {
            Health += amount;
        }
        
        if(Health > MaxHealth) Health = MaxHealth;
        
        HudController.Instance.UpdateHUD();
    }

    public void TakingAmmo(int amount, AmmoType ammoType)
    {
        GameManager.Instance.powerUpEffect();

        if (ammoType == AmmoType.Bullet)
        {
            BulletsCounter += amount;
            if(BulletsCounter > 200) BulletsCounter = 200;
            
            if(PlayerShooting.Instance.currentGunData.bullets) PlayerShooting.Instance.currentGunData.currentAmmo = BulletsCounter;
        }

        if (ammoType == AmmoType.Shell)
        {
            ShellCounter += amount;
            if(ShellCounter > 100) ShellCounter = 100;
            
            if(PlayerShooting.Instance.currentGunData.shells) PlayerShooting.Instance.currentGunData.currentAmmo = ShellCounter;
        }

        if (ammoType == AmmoType.Rocket)
        {
            RocketsCounter += amount;
            if(RocketsCounter > 100) RocketsCounter = 100;
            
            if(PlayerShooting.Instance.currentGunData.rockets) PlayerShooting.Instance.currentGunData.currentAmmo = RocketsCounter;
        }

        if (ammoType == AmmoType.Cell)
        {
            CellsCounter += amount;
            if(CellsCounter > 300) CellsCounter = 300;
            
            if(PlayerShooting.Instance.currentGunData.cells) PlayerShooting.Instance.currentGunData.currentAmmo = CellsCounter;
        }
        
        HudController.Instance.UpdateHUD();
    }
    
    public int GetHealth() => Health;
    public int GetArmor() => Armor;
    
}

public enum AmmoType
{
    Bullet,
    Shell,
    Rocket,
    Cell
}
