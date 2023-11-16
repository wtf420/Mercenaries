using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineProducer : IWeapon
{
	#region Fields & Properties
	private float currentBulletQuantity;
	#endregion

	#region Methods
	public override void Initialize()
	{
		Type = GameConfig.WEAPON.MINE_PRODUCER;
		currentBulletQuantity = 3;
	}

	public override void AttemptAttack()
	{
		if (currentBulletQuantity > 0)
		{
			Attack();
		}
		else if (currentBulletQuantity <= 0)
		{
			
		}
	}

	protected void Attack()
	{
		Debug.Log("Spawn Mine");
		if (currentBulletQuantity > 0)
		{
			Debug.Log("Spawn Mine");
			// spawn mine
			Mine mine = Mine.Create(transform.position, this.tag);

			currentBulletQuantity -= 1;
			if (currentBulletQuantity == 0)
			{

			}
		}
	}

	#endregion
}

