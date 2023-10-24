using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gun: Weapon
{
	#region Fields & Properties
	[SerializeField] Bullet bulletPrefab;
	[SerializeField] float inaccuracy;

	private AudioSource gunSound;

	#endregion

	#region Methods
	public override void Initialize(Transform parent = null, SO_WeaponGunStats gunStats = null)
	{
		Type = GameConfig.WEAPON.RIFLE;
		base.Initialize(parent);
		currentBulletQuantity = (int)Stats[WEAPON_STAT_TYPE.QUANTITY];

		gunSound = GetComponent<AudioSource>();
	}

	protected override void Attack()
	{
		base.Attack();

		if (currentBulletQuantity > 0)
		{
			// spawn bullet
			Vector3 direction = (transform.forward).normalized;
			Vector3 target = direction * 10f + Vector3.Cross(direction, transform.up).normalized * UnityEngine.Random.Range(-inaccuracy, inaccuracy);
			direction = (target).normalized;

			Bullet bullet = Instantiate(bulletPrefab, transform.position, new Quaternion());
			bullet.Initialize(Stats[WEAPON_STAT_TYPE.DAMAGE],
							  Stats[WEAPON_STAT_TYPE.ATTACK_RANGE],
							  Stats[WEAPON_STAT_TYPE.SPEED],
							 direction);
			bullet.tag = this.tag;

			gunSound.Stop();
			gunSound.Play();

			currentBulletQuantity -= 1;
			if (currentBulletQuantity == 0)
				StartCoroutine(IE_Reload());
		}
	}

	#endregion
}

