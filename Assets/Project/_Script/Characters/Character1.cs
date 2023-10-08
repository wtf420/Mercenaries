using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character1 : Character
{
	#region Fields & Properties

	#endregion

	#region Methods

	public override void Initialize()
	{
		base.Initialize();
	}

	public override void UpdateCharacter(List<Enemy> enemies = null)
	{
		base.UpdateCharacter();

		if(myPet != null)
		{
			myPet.UpdatePet(enemies);
		}
	}

	public override void KeyboardController()
	{
		base.KeyboardController();

		if (myPet != null)
		{
			return;
		}

		// Summon pet
		if(Input.GetKeyDown(KeyCode.R))
		{
			myPet = Drone.Create(transform);
			myPet.Initialize(this.tag);

			Debug.Log("SUMMON DRONE");
		}
	}


	#endregion
}

