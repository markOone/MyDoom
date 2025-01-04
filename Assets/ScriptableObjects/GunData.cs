using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [SerializeField] internal bool is2D;
    
    [Header("Shooting")]
    public int damage;
    public float wideRange;
    public float lengthRange;

    [Header("Reloading")]
    public int currentAmmo = 0;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    [HideInInspector] public bool reloading;
    [SerializeField] internal bool shells;
    [SerializeField] internal bool bullets;
    [SerializeField] internal bool rockets;
    [SerializeField] internal bool cells;


}
