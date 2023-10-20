using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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

public class GameManager: MonoBehaviour
{
	#region Fields & Properties
	public SO_CharacterDefault characterStat;
	public List<WeaponSO> weaponStats;
	public List<PetSO> petStats;

	private static GameManager instance;
	public static GameManager Instance
	{
		get => instance;
		private set => instance = value;
	}
	#endregion

	#region Methods
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	public ScriptableObject GetStats(Weapon weapon)
	{
		//Debug.Log($"Type: {type}, Index {index}");
		return weaponStats.Find(element => element.weapon.GetType() == weapon.GetType()).Stats;
	}

	public ScriptableObject GetStats(Pet pet)
	{
		//Debug.Log($"Type: {type}, Index {index}");
		return petStats.Find(element => element.pet.GetType() == pet.GetType()).Stats;
	}

	// public ScriptableObject GetStats(GameConfig.SO_TYPE type, int index = 0)
	// {
	// 	//Debug.Log($"Type: {type}, Index {index}");
	// 	return SO_Stats.Find(element => element.Type == type).Stats[index];
	// }
	#endregion
}
