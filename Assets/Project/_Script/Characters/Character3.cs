using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character3 : Character
{
	#region Fields & Properties

	#endregion

	#region Methods
	new public static Character3 Create(Transform parent, Vector3 position)
	{
		Character3 character = Instantiate(Resources.Load<Character3>("_Prefabs/Characters/Character 3"), parent);
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
			if (!myPet.IsDeath)
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

