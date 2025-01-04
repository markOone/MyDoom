using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public int CellsCounter = 0;

    public static PlayerStats Instance { get { if (_instance == null) Debug.Log("No PlayerStats"); return _instance; } }
    
    private void OnEnable()
    {
        _instance = this;
    }

    void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        Health = MaxHealth;
        Armor = 0;
        HudController.Instance.UpdateHUD();
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        HudController.Instance.UpdateHUD();
        if (Health <= 0)
        {
            Invoke("GameManager.Instance.RestartGame", 2f);
        }
    }

    public void Heal(int heal)
    {
        Health += heal;
        HudController.Instance.UpdateHUD();
    }

    public void TakeArmor(int armor)
    {
        Armor += armor;
        HudController.Instance.UpdateHUD();
    }

    public void TakeBullets(int ammo)
    {
        BulletsCounter += ammo;
        if(this.GetComponent<PlayerShooting>().currentGunData.bullets) this.GetComponent<PlayerShooting>().currentGunData.currentAmmo += ammo;
        HudController.Instance.UpdateHUD();
    }

    public void TakeShells(int ammo)
    {
        ShellCounter += ammo;
        if(this.GetComponent<PlayerShooting>().currentGunData.shells) this.GetComponent<PlayerShooting>().currentGunData.currentAmmo += ammo;
        HudController.Instance.UpdateHUD();
    }

    public void TakeRockets(int ammo)
    {
        RocketsCounter += ammo;
        if(this.GetComponent<PlayerShooting>().currentGunData.rockets) this.GetComponent<PlayerShooting>().currentGunData.currentAmmo += ammo;
        HudController.Instance.UpdateHUD();
    }

    public void TakeCells(int ammo)
    {
        CellsCounter += ammo;
        if(this.GetComponent<PlayerShooting>().currentGunData.cells) this.GetComponent<PlayerShooting>().currentGunData.currentAmmo += ammo;
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
