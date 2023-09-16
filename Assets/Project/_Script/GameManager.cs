using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
	#region Fields & Properties
	public SO_CharacterDefault CharacterDefaultStats;

	[SerializeField] Character character;

	#endregion

	#region Methods
	private void Start()
	{
		character.Intialize(CharacterDefaultStats);
	}

	private void Update()
	{
		character.UpdateCharacter();
	}
	#endregion
}
