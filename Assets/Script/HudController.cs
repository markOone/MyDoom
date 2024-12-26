using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject HUD;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI ArmorText;
    public TextMeshProUGUI AmmoText;
    
    public TextMeshProUGUI BulletText;
    public TextMeshProUGUI ShellText;
    public TextMeshProUGUI RocketText;
    

    private static HudController _instance;

    public static HudController Instance { get { if (_instance == null) Debug.Log("No HUDController"); return _instance; } }

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHUD(int health, int armor, int bullet, int shell, int rocket)
    {
        HealthText.text = health.ToString();
        ArmorText.text = armor.ToString();
        BulletText.text = bullet.ToString();
        ShellText.text = shell.ToString();
        RocketText.text = rocket.ToString();
        
        //Check what ammo shoul be displayed dependent on gun player holds
    }
}
