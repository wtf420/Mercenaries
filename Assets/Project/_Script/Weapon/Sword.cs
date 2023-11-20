using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sword : IWeapon
{
	#region Fields & Properties
	[SerializeField] protected SO_WeaponGunStats soStats;

	private bool _isAttackable;
	#endregion

	#region Methods
	public override void Initialize()
	{
		_isAttackable = true;
	}

	public override void AttemptAttack()
	{
		StartCoroutine(Attack());
	}

	protected IEnumerator Attack()
	{
		_isAttackable = true;
		yield return new WaitForSeconds(1f / soStats.ATTACK_SPEED_DEFAULT);
		_isAttackable = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!_isAttackable)
		{
			return;
		}

		if (CompareTag(other.tag))
		{
			return;
		}

		IDamageable target = other.GetComponent<IDamageable>();
		if (target != null)
		{
			target.TakenDamage(soStats.DAMAGE_DEFAULT);
			_isAttackable = false;
		}
	}

	#endregion
}

