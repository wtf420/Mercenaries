using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
	#region Fields & Properties
	private static GameManager instance;
	public static GameManager Instance
	{
		get => instance;
		private set => instance = value;
	}

	public List<ScriptableObject> CharacterStats;

	public List<ScriptableObject> WeaponStats;

	[SerializeField] Character character;

	#endregion

	#region Methods
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		character.Intialize();
	}

	private void Update()
	{
		character.UpdateCharacter();
	}
	#endregion
}
