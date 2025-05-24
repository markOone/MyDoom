using System;
using UnityEngine;
using MyDoom.Player;
using MyDoom.ShootingSystem;
using MyDoom.GeneralSystems;
using Random = UnityEngine.Random;

public class PlayerStats : MonoBehaviour, IDamagable
{
    public event EventHandler<HealthChangedEventArgs> OnHealthChanged;
    public event EventHandler<ArmorChangedEventArgs> OnArmorChanged;
    public event EventHandler<AmmoChangedEventArgs> OnAmmoChanged;
    public event EventHandler OnPlayerDeath;
    
    [Header("Object")]
    private static PlayerStats _instance;
    
    [Header("References")]
    [SerializeField] Animator damageAnimator;
    [SerializeField] AudioSource audioSource;
    
    [Header("Player Stats")]
    int _health;
    int MaxHealth = 100;

    int _armor;
    int MaxArmor = 200;

    [SerializeField]private int _bulletsCounter = 0;
    [SerializeField]private int _shellCounter = 0;
    [SerializeField]private int _rocketsCounter = 0;
    [SerializeField]private int _cellsCounter = 0;

    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            OnHealthChanged?.Invoke(this, new HealthChangedEventArgs(_health));
        }
    }
    
    public int Armor
    {
        get => _armor;
        set
        {
            _armor = value;
            OnArmorChanged?.Invoke(this, new ArmorChangedEventArgs(_armor));
        }
    }
    
    
    public int BulletsCounter
    {
        get => _bulletsCounter;
        set
        {
            _bulletsCounter = value;
            OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(AmmoType.Bullet, _bulletsCounter));
        }
    }
    
    public int ShellCounter
    {
        get => _shellCounter;
        set
        {
            _shellCounter = value;
            OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(AmmoType.Shell, _shellCounter));
        }
    }
    
    public int RocketsCounter
    {
        get => _rocketsCounter;
        set
        {
            _rocketsCounter = value;
            OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(AmmoType.Rocket, _rocketsCounter));
        }
    }
    
    public int CellsCounter
    {
        get => _cellsCounter;
        set
        {
            _cellsCounter = value;
            OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(AmmoType.Cell, _cellsCounter));
        }
    }
    
    public static PlayerStats Instance
    {
        get
        {
            if (_instance == null) Debug.Log("No PlayerStats"); 
            return _instance;
        }
    }
    
    private void OnEnable()
    {
        _instance = this;
    }

    void Awake()
    {
        _instance = this;
        audioSource = GetComponent<AudioSource>();
        _health = MaxHealth;
        _armor = 0;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(this, new HealthChangedEventArgs(_health));
        OnArmorChanged?.Invoke(this, new ArmorChangedEventArgs(_armor));
        OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(AmmoType.Bullet, BulletsCounter));
        OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(AmmoType.Shell, ShellCounter));
        OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(AmmoType.Rocket, RocketsCounter));
        OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(AmmoType.Cell, CellsCounter));
        //HudController.Instance.UpdateHUD();
    }

    private void FixedUpdate()
    {
        if(_health <= 0) OnPlayerDeath?.Invoke(this, EventArgs.Empty);
    }

    public void TakeDamage(int damage)
    {
        audioSource?.Play(0);
        int randomizedDamage = damage * Random.Range(0, 4);
        if (_armor > 0)
        {
            if (_armor >= randomizedDamage) _armor -= randomizedDamage;
            else
            {
                int remainingDamage = randomizedDamage - _armor; 
                _health -= remainingDamage;
                _armor = 0;
            }
        }
        else
        {
            _health -= randomizedDamage;
        }
        
        damageAnimator.SetTrigger("Damage");
        OnHealthChanged?.Invoke(this, new HealthChangedEventArgs(_health));
        OnArmorChanged?.Invoke(this, new ArmorChangedEventArgs(_armor));
        //HudController.Instance.UpdateHUD();
        if (_health <= 0)
        {
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    public void TakeArmor(int amount)
    {
        GameManager.Instance.powerUpEffect();

        if (amount == 100)
        {
            _armor += amount;
            if(_armor < 100) _armor = 100;
        }
        else
        {
            _armor += amount;
        }
        
        if(_armor > MaxArmor) _armor = MaxArmor;
        
        // HudController.Instance.UpdateHUD();
        OnArmorChanged?.Invoke(this, new ArmorChangedEventArgs(_armor));
    }
    
    public void TakeHealthKit(int amount)
    {
        GameManager.Instance.powerUpEffect();
        
        if (amount is 10 or 25)
        {
            _health += amount;
            if(_health < 100) _armor = 100;
        }
        else
        {
            _health += amount;
        }
        
        if(_health > MaxHealth) _health = MaxHealth;
        
        // HudController.Instance.UpdateHUD();
        OnHealthChanged?.Invoke(this, new HealthChangedEventArgs(_health));
    }

    public void TakeAmmo(int amount, AmmoType ammoType)
    {
        GameManager.Instance.powerUpEffect();

        if (ammoType == AmmoType.Bullet)
        {
            BulletsCounter += amount;
            if(BulletsCounter > 200) BulletsCounter = 200;
            
            if(PlayerShooting.Instance.currentGunData.bullets) PlayerShooting.Instance.currentGunData.currentAmmo = BulletsCounter;
            OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(ammoType, BulletsCounter));
        }

        if (ammoType == AmmoType.Shell)
        {
            ShellCounter += amount;
            if(ShellCounter > 100) ShellCounter = 100;

            if(PlayerShooting.Instance.currentGunData.shells) PlayerShooting.Instance.currentGunData.currentAmmo = ShellCounter;
            OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(ammoType, ShellCounter));
        }

        if (ammoType == AmmoType.Rocket)
        {
            RocketsCounter += amount;
            if(RocketsCounter > 100) RocketsCounter = 100;
            
            if(PlayerShooting.Instance.currentGunData.rockets) PlayerShooting.Instance.currentGunData.currentAmmo = RocketsCounter;
            OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(ammoType, RocketsCounter));
        }

        if (ammoType == AmmoType.Cell)
        {
            CellsCounter += amount;
            if(CellsCounter > 300) CellsCounter = 300;
            
            if(PlayerShooting.Instance.currentGunData.cells) PlayerShooting.Instance.currentGunData.currentAmmo = CellsCounter;
            OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(ammoType, CellsCounter));
        }
        
        //HudController.Instance.UpdateHUD();
        // OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(ammoType, amount));

    }
    
    public void DecreaseAmmo(int amount, AmmoType ammoType)
    {
        if (ammoType == AmmoType.Bullet)
        {
            BulletsCounter -= amount;
            if(BulletsCounter < 0) BulletsCounter = 0;
            OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(ammoType, BulletsCounter));
        }

        if (ammoType == AmmoType.Shell)
        {
            ShellCounter -= amount;
            if(ShellCounter < 0) ShellCounter = 0;
            OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(ammoType, ShellCounter));
        }

        if (ammoType == AmmoType.Rocket)
        {
            RocketsCounter -= amount;
            if(RocketsCounter < 0) RocketsCounter = 0;
            OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(ammoType, RocketsCounter));
        }

        if (ammoType == AmmoType.Cell)
        {
            CellsCounter -= amount;
            if(CellsCounter < 0) CellsCounter = 0;
            OnAmmoChanged?.Invoke(this, new AmmoChangedEventArgs(ammoType, CellsCounter));
        }
    }
    
    public int GetHealth() => _health;
    public int GetArmor() => _armor;

    public void Damage(float damage, float distance)
    {
        TakeDamage((int)damage);
    }
}


