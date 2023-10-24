using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataBank : MonoBehaviour
{
    [SerializeField] public List<CharacterSO> characterStats;
    [SerializeField] public List<WeaponSO> weaponStats;
    [SerializeField] public List<PetSO> petStats;
    [SerializeField] public List<EnemyDicSO> EnemyStats;
}

[Serializable]
public class EnemyDicSO
{
    [SerializeField] public Enemy enemy;
    [SerializeField] public SO_EnemyDefault Stats;
}

[Serializable]
public class CharacterSO
{
    [SerializeField] public Character characterType;
    [SerializeField] public GameObject characterPrefab;
    [SerializeField] public SO_CharacterDefault characterStats;
}

[Serializable]
public class WeaponSO
{
    [SerializeField] public Weapon weapon;
    [SerializeField] public SO_WeaponGunStats Stats;
}

[Serializable]
public class PetSO
{
    [SerializeField] public Pet pet;
    [SerializeField] public ScriptableObject Stats;
}
