using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Object")]
    private static PlayerStats _instance;
    
    [Header("Player Stats")]
    int Health;
    int MaxHealth = 100;

    int Armor;
    int MaxArmor = 200;
    

    public int BulletsCounter = 0;
    public int ShellCounter = 0;
    public int RocketsCounter = 0;

    public static PlayerStats Instance { get { if (_instance == null) Debug.Log("No Player"); return _instance; } }
    
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        Health = MaxHealth;
        Armor = 0;
        HudController.Instance.UpdateHUD(Health, Armor, BulletsCounter, ShellCounter, RocketsCounter);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        HudController.Instance.UpdateHUD(Health, Armor, BulletsCounter, ShellCounter, RocketsCounter);
        if (Health <= 0)
        {
            Invoke("GameManager.Instance.RestartGame", 2f);
        }
    }

    public void Heal(int heal)
    {
        Health += heal;
        HudController.Instance.UpdateHUD(Health, Armor, BulletsCounter, ShellCounter, RocketsCounter);
    }

    public void TakeArmor(int armor)
    {
        Armor += armor;
        HudController.Instance.UpdateHUD(Health, Armor, BulletsCounter, ShellCounter, RocketsCounter);
    }

    public void TakeBullets(int ammo)
    {
        BulletsCounter += ammo;
        HudController.Instance.UpdateHUD(Health, Armor, BulletsCounter, ShellCounter, RocketsCounter);
    }

    public void TakeShells(int ammo)
    {
        ShellCounter += ammo;
        HudController.Instance.UpdateHUD(Health, Armor, BulletsCounter, ShellCounter, RocketsCounter);
    }

    public void TakeRockets(int ammo)
    {
        RocketsCounter += ammo;
        HudController.Instance.UpdateHUD(Health, Armor, BulletsCounter, ShellCounter, RocketsCounter);
    }
}
