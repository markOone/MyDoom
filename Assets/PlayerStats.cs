using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    int Health;
    int MaxHealth = 100;

    int Armor;
    int MaxArmor = 200;
    

    int BulletsCounter = 0;
    int ShellCounter = 0;
    int RocketsCounter = 0;

    private void Start()
    {
        Health = MaxHealth;
        Armor = 0;
        HudController.Instance.UpdateHUD(Health, Armor, BulletsCounter, ShellCounter, RocketsCounter);
    }
}
