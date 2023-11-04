using System;
using System.Collections.Generic;
using UnityEngine;

public class Character4 : Character
{
	public static Character4 Create(Transform parent, Vector3 position)
	{
		Character4 character = Instantiate(Resources.Load<Character4>("_Prefabs/Characters/Character 4"), parent);
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

	public override void SwapWeapon()
	{
		//	Use bulletproof Wall
		if (Input.GetKey(KeyCode.Alpha2))
		{
			var wall = BulletproofWall.Create(null, transform.position, weapons[0]._Weapon.transform.rotation);
			wall.Initialize();
			wall.tag = this.tag;
		}
	}

}
