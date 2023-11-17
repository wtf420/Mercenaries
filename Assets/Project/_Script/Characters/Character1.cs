using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character1 : Character
{
	#region Fields & Properties

	#endregion

	#region Methods
	new public static Character1 Create(Transform parent, Vector3 position)
	{
		Character1 character = Instantiate(Resources.Load<Character1>("_Prefabs/Characters/Character 1"), parent);
		character.transform.position = position;

		return character;
	}

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
			//return;
		}

		// Summon pet
		if(Input.GetKeyDown(KeyCode.R))
		{
			Drone drone = new Drone(this.gameObject);

			Debug.Log("SUMMON DRONE");
		}
	}


	#endregion
}

