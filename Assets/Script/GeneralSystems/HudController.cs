using System;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using MyDoom.GeneralSystems;
using MyDoom.ShootingSystem;

public class HudController : MonoBehaviour
{
    [Header("UI Elements")] 
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI ArmorText;
    public TextMeshProUGUI AmmoText;
    
    public TextMeshProUGUI BulletText;
    public TextMeshProUGUI ShellText;
    public TextMeshProUGUI RocketText;
    public TextMeshProUGUI CellsText;
    

    private static HudController _instance;

    public static HudController Instance
    {
        get
        {
            if (_instance == null) Debug.Log("No HUDController"); 
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        PlayerStats.Instance.OnHealthChanged += UpdateHealthUI;
        PlayerStats.Instance.OnArmorChanged += UpdateArmorUI;
        PlayerStats.Instance.OnAmmoChanged += UpdateAmmoUI;
        
        // Force initial update
        UpdateHealthUI(null, new HealthChangedEventArgs(PlayerStats.Instance.Health));
        UpdateArmorUI(null, new ArmorChangedEventArgs(PlayerStats.Instance.Armor));
        UpdateAmmoUI(null, new AmmoChangedEventArgs(AmmoType.Bullet, PlayerStats.Instance.BulletsCounter));
        UpdateAmmoUI(null, new AmmoChangedEventArgs(AmmoType.Shell, PlayerStats.Instance.ShellCounter));
        UpdateAmmoUI(null, new AmmoChangedEventArgs(AmmoType.Rocket, PlayerStats.Instance.RocketsCounter));
        UpdateAmmoUI(null, new AmmoChangedEventArgs(AmmoType.Cell, PlayerStats.Instance.CellsCounter));
    }
    
    private void OnDisable()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnHealthChanged -= UpdateHealthUI;
            PlayerStats.Instance.OnArmorChanged -= UpdateArmorUI;
            PlayerStats.Instance.OnAmmoChanged -= UpdateAmmoUI;
        }
    }

    void UpdateHealthUI([CanBeNull] object sender, HealthChangedEventArgs eventArgs)
    {
        HealthText.text = eventArgs.Health.ToString() + "%";
    }
    
    void UpdateArmorUI([CanBeNull] object sender, ArmorChangedEventArgs eventArgs)
    {
        ArmorText.text = eventArgs.Armor.ToString() + "%";
    }
    
    void UpdateAmmoUI([CanBeNull] object sender, AmmoChangedEventArgs eventArgs)
    {
        if(eventArgs.Type == AmmoType.Bullet) BulletText.text = eventArgs.Ammo.ToString();
        if(eventArgs.Type == AmmoType.Shell) ShellText.text = eventArgs.Ammo.ToString();
        if(eventArgs.Type == AmmoType.Rocket) RocketText.text = eventArgs.Ammo.ToString();
        if(eventArgs.Type == AmmoType.Cell) CellsText.text = eventArgs.Ammo.ToString();
    }
}
