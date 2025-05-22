using System;
using System.Collections;
using System.Collections.Generic;
using MyDoom.GeneralSystems;
using UnityEngine;
using UnityEngine.InputSystem;
using MyDoom.Player;
using MyDoom.ShootingSystem;

public class WeaponSwitching : MonoBehaviour
{
    public event EventHandler<AmmoChangedEventArgs> OnWeaponSelectedEvent;
    
    [Header("References")]
    [SerializeField] List<Transform> weapons;
    [SerializeField] Transform shotgun;

    [Header("Key")] 
    [SerializeField] private InputAction switchBind;
    
    
    [Header("Settings")]
    [SerializeField] float switchTime;

    private int selectedWeapon = 1;
    private float timeSinceLastSwitch;
    
    private void OnEnable()
    {
        switchBind.Enable();
    }

    private void OnDisable()
    {
        switchBind.Disable();
    }
    
    private void Start()
    {
        SetGunActive(selectedWeapon);
        timeSinceLastSwitch = 0f;
    }

    private void Update()
    {

        if (switchBind.IsPressed())
        {
            Select(selectedWeapon);
        }
        
        timeSinceLastSwitch += Time.deltaTime;
    }

    private void Select(int weaponIndex)
    {
        if (timeSinceLastSwitch >= switchTime)
        {
            int pressedBindingIndex = GetPressedBindingIndex(switchBind);
            selectedWeapon = pressedBindingIndex;
            if(pressedBindingIndex != -1) SetGunActive(pressedBindingIndex);
                
            timeSinceLastSwitch = 0f;
            
            OnWeaponSelected();
        }
    }

    void SetGunActive(int gunIndex)
    {
        GunData chosenGunGunData;
        TransformIterator iterator = new TransformIterator(weapons);
        iterator.Reset();
        while (iterator.MoveNext())
        {
            iterator.Current.gameObject.SetActive(false);
        }
        shotgun.gameObject.SetActive(false);

        if (gunIndex == 2)
        {
            shotgun.gameObject.SetActive(true);
            PlayerShooting.Instance.currentGunData = shotgun.GetChild(1).gameObject.GetComponent<Gun>().gunData;
            PlayerShooting.Instance.currentGun = shotgun.GetChild(1).gameObject.GetComponent<Gun>();
            chosenGunGunData = shotgun.GetChild(1).gameObject.GetComponent<Gun>().gunData;
        }
        else
        {
            weapons[gunIndex].gameObject.SetActive(true);
            PlayerShooting.Instance.currentGun = weapons[gunIndex].gameObject.GetComponent<Gun>();
            PlayerShooting.Instance.currentGunData = weapons[gunIndex].gameObject.GetComponent<Gun>().gunData;
            chosenGunGunData = weapons[gunIndex].gameObject.GetComponent<Gun>().gunData;
        }
        
        PlayerShooting.Instance.currentGun.CheckAmmoFromPlayerStats();
    }

    private void OnWeaponSelected()
    {
        Debug.Log("Weapon Changed");
    }

    int GetPressedBindingIndex(InputAction action)
    {
        int result = -1;
        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (action.controls[i].IsPressed())
            {
                result = i;
            }
        }
        
        return result;
    }
}

public class TransformIterator : IEnumerator<Transform>
{
    private readonly List<Transform> transforms;
    private int position = -1;

    public TransformIterator(List<Transform> transforms)
    {
        this.transforms = transforms;
    }

    public Transform Current => transforms[position];
    object IEnumerator.Current => transforms[position];

    public bool MoveNext() => ++position < transforms.Count;
    public void Reset() => position = -1;
    public void Dispose() { }

    public Transform GetNextTransform()
    {
        position = (position + 1) % transforms.Count;
        return transforms[position];
    } 
}
