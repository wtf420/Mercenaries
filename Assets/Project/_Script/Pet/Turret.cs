using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : Pet
{
	#region Fields & Properties
	[SerializeField] private Bullet bulletPrefab;

	#endregion

	#region Methods
	public static Turret Create(Transform parent = null)
	{
		Turret turret = Instantiate(Resources.Load<Turret>("_Prefabs/Pet/Turret"), parent);

		return turret;
	}

	public override void Initialize(string tag)
	{
		Type = GameConfig.PET.TURRET;
		base.Initialize(tag);
	}

	public override void UpdatePet(List<Enemy> enemies = null)
	{
		base.UpdatePet(enemies);
	}

	protected override void Attack()
	{
		base.Attack();

		if (!target)
		{
			return;
		}

		if (attackable)
		{
			Bullet bullet = Instantiate(bulletPrefab, transform.position, new Quaternion());
			bullet.Initialize(Stats[GameConfig.STAT_TYPE.DAMAGE],
							  Stats[GameConfig.STAT_TYPE.ATTACK_RANGE],
							  Stats[GameConfig.STAT_TYPE.BULLET_SPEED],
							 (target.position - transform.position).normalized);
			bullet.tag = this.tag;

			attackable = false;
			StartCoroutine(IE_Reload());
		}
	}

	public override void TakenDamage(float damage)
	{
		base.TakenDamage(damage);
		if(Stats[GameConfig.STAT_TYPE.HP] > 0)
		{
			Stats[GameConfig.STAT_TYPE.HP] -= damage;
			if(Stats[GameConfig.STAT_TYPE.HP] <= 0)
			{
				IsDeath = true;
			}
		}
	}

	#endregion
}
