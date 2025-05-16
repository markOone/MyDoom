using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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

    public static HudController Instance { get { if (_instance == null) Debug.Log("No HUDController"); return _instance; } }

    void Awake()
    {
        _instance = this;
    }
    
    public void UpdateHUD()
    {
        
        HealthText.text = PlayerStats.Instance.GetHealth().ToString() + "%";
        ArmorText.text = PlayerStats.Instance.GetArmor().ToString() + "%";
        BulletText.text = PlayerStats.Instance.BulletsCounter.ToString();
        ShellText.text = PlayerStats.Instance.ShellCounter.ToString();
        RocketText.text = PlayerStats.Instance.RocketsCounter.ToString();
        CellsText.text = PlayerStats.Instance.CellsCounter.ToString();
        
        //Check what ammo shoul be displayed dependent on gun player holds
    }
}
