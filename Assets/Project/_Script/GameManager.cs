using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager: MonoBehaviour
{
	#region Fields & Properties

	//public DataBank DataBank;
	//public CharacterSO selectedCharacter = null;
	public GameConfig.CHARACTER SelectedCharacter;

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
		DontDestroyOnLoad(gameObject);

		SelectedCharacter = GameConfig.CHARACTER.CHARACTER_4;
	}

	public void BeginLevel(string levelname)
	{
		SceneManager.LoadScene(levelname);
	}

	//public ScriptableObject GetStats(Weapon weapon)
	//{
	//	//Debug.Log($"Type: {type}, Index {index}");
	//	return DataBank.weaponStats.Find(element => element.weapon.GetType() == weapon.GetType()).Stats;
	//}

	//public ScriptableObject GetStats(Pet pet)
	//{
	//	//Debug.Log($"Type: {type}, Index {index}");
	//	return DataBank.petStats.Find(element => element.pet.GetType() == pet.GetType()).Stats;
	//}

	// public ScriptableObject GetStats(GameConfig.SO_TYPE type, int index = 0)
	// {
	// 	//Debug.Log($"Type: {type}, Index {index}");
	// 	return SO_Stats.Find(element => element.Type == type).Stats[index];
	// }
	#endregion
}
