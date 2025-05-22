using System;
using MyDoom.ShootingSystem;

namespace MyDoom.GeneralSystems
{
    public sealed class HealthChangedEventArgs : EventArgs
    {
        public int Health{get;}

        public HealthChangedEventArgs(int health)
        {
            Health = health;
        }
    }

    public sealed class ArmorChangedEventArgs : EventArgs
    {
        public int Armor{get;}

        public ArmorChangedEventArgs(int armor)
        {
            Armor = armor;
        }
    }

    public sealed class AmmoChangedEventArgs : EventArgs
    {
        public int Ammo{get;}
        public AmmoType Type{get;}

        public AmmoChangedEventArgs(AmmoType type, int ammo)
        {
            Ammo = ammo;
            Type = type;
        }
    }
}