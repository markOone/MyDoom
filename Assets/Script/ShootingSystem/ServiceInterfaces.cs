using UnityEngine;

namespace MyDoom.ShootingSystem
{
    public interface IWeaponSystem
    {
        void Shoot(WeaponContext context);
    }

    public interface IDamageHandler
    {
        void HandleDamage(DamageContext context);
    }

    public interface IAutoAimSystem
    {
        GameObject ChooseEnemyWithAutoAim(AutoAimContext context);
    }
}