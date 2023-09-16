using UnityEngine;

[CreateAssetMenu(menuName = "Character Data/Character Default Stats")]
public class SO_CharacterDefault : ScriptableObject
{
    public float HP_DEFAULT = 150;

    public float DAMAGE_DEFAULT = 20;

    public float ATTACK_RANGE_DEFAULT = 2;
    public float ATTACK_SPEED_DEFAULT = 2; // x times per second 

    public float MOVE_SPEED_DEFAULT = 6;
}

