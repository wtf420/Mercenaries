using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Data/Rifle Stats")]
public class SO_WeaponRifle: ScriptableObject
{
    public float DAMAGE_DEFAULT = 20;

    public float ATTACK_RANGE_DEFAULT = 10;
    public float ATTACK_SPEED_DEFAULT = 10; // x times per second 

    public float SPEED_DEFAULT = 6;

    public float RELOAD_TIME = 1;

    public float BULLET_QUANTITY = 30;
}

