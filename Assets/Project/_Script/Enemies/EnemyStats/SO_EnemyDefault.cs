using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Data/Enemy Default Stats")]
class SO_EnemyDefault: ScriptableObject
{
    public float HP_DEFAULT = 100;

    public float DAMAGE_DEFAULT = 20;

    public float ATTACK_RANGE_DEFAULT = 2;
    public float ATTACK_SPEED_DEFAULT = 2; // x times per second 

    public float MOVE_SPEED_DEFAULT = 6;

    public float DETECT_RANGE = 6f;
}

