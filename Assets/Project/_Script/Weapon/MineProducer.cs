using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineProducer : Weapon
{
	#region Fields & Properties

	#endregion

	#region Methods
	public override void Initialize(Transform parent = null)
	{
		base.Initialize(parent);

		Type = GameConfig.WEAPON.MINE_PRODUCER;

		currentBulletQuantity = (int)Stats[WEAPON_STAT_TYPE.QUANTITY];

	}

	protected override void Attack()
	{
		base.Attack();
		Debug.Log("Spawn Mine");
		if (currentBulletQuantity > 0)
		{
			Debug.Log("Spawn Mine");
			// spawn mine
			Mine mine = Mine.Create(transform.position, this.tag);

			currentBulletQuantity -= 1;
			if (currentBulletQuantity == 0)
				StartCoroutine(IE_Reload());
		}
	}

	#endregion
}

