using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character2 : Character
{
	#region Fields & Properties

	#endregion

	#region Methods
	new public static Character2 Create(Transform parent, Vector3 position)
	{
		Character2 character = Instantiate(Resources.Load<Character2>("_Prefabs/Characters/Character 2"), parent);
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

		if (myPet != null)
		{
			if(!myPet.IsDead)
			{
				myPet.UpdatePet(enemies);
			}
			else
			{
				Destroy(myPet.gameObject);
				myPet = null;
			}
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
		if (Input.GetKeyDown(KeyCode.R))
		{
			myPet = Turret.Create();
			myPet.transform.position = transform.position;
			myPet.Initialize(this.tag);

			Debug.Log("SUMMON Turret");
		}
	}


	#endregion
}

