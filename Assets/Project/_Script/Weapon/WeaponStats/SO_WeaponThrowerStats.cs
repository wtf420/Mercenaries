using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Data/Thrower Stats")]
public class SO_WeaponThrowerStats : ScriptableObject
{
    public float DAMAGE_DEFAULT = 20;
    public float ATTACK_RANGE_DEFAULT = 10;
    public float ATTACK_SPEED_DEFAULT = 10; // x times per second
    public float COOLDOWN = 1;
    public float THROWFORCE = 10;
    public int MAX_GERNADE_COUNT = 3;
}

