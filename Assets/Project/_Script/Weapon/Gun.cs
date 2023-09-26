using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gun: Weapon
{
	#region Fields & Properties
	[SerializeField] Bullet bulletPrefab;

	#endregion

	#region Methods
	public override void Initialize(Transform parent = null)
	{
		base.Initialize(parent);

		SO_WeaponGunStats stats = (SO_WeaponGunStats)GameManager.Instance.GetStats(GameConfig.SO_TYPE.WEAPON, (int)GameConfig.WEAPON.RIFLE);
		Stats.Add(WEAPON_STAT_TYPE.DAMAGE, stats.DAMAGE_DEFAULT);
		Stats.Add(WEAPON_STAT_TYPE.ATTACK_RANGE, stats.ATTACK_RANGE_DEFAULT);
		Stats.Add(WEAPON_STAT_TYPE.ATTACK_SPEED, stats.ATTACK_SPEED_DEFAULT);
		Stats.Add(WEAPON_STAT_TYPE.SPEED, stats.SPEED_DEFAULT);
		Stats.Add(WEAPON_STAT_TYPE.RELOAD_TIME, stats.RELOAD_TIME);
		Stats.Add(WEAPON_STAT_TYPE.QUANTITY, stats.BULLET_QUANTITY);

		currentBulletQuantity = (int)Stats[WEAPON_STAT_TYPE.QUANTITY];
	}

	protected override void Attack()
	{
		base.Attack();

		if(currentBulletQuantity > 0)
		{
			// spawn bullet
			Bullet bullet = Instantiate(bulletPrefab, transform.position, new Quaternion());
			bullet.Initialize(Stats[WEAPON_STAT_TYPE.DAMAGE],
							  Stats[WEAPON_STAT_TYPE.ATTACK_RANGE],
							  Stats[WEAPON_STAT_TYPE.SPEED],
							 (transform.position - transform.parent.position).normalized);
			bullet.tag = this.tag;

			currentBulletQuantity -= 1;
			if (currentBulletQuantity == 0)
				StartCoroutine(IE_Reload());
		}
	}

	#endregion
}

