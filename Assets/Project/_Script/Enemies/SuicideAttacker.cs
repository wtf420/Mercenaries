using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SuicideAttacker : Enemy
{
	#region Fields & Properties

	#endregion

	#region Methods

	public override void Initialize(Path p = null)
	{
		base.Initialize(p);
	}

	public override void UpdateEnemy(Character character)
	{
		Debug.LogWarning(target);
		if (target)
		{
			if (Vector3.Distance(transform.position, target.position)
			<= Stats[GameConfig.STAT_TYPE.ATTACK_RANGE])
			{
				Explode();

				return;
			}

			FindTarget();

			return;
		}

		if (!IsDetectSuccessful(character))
		{
			MovementBehaviour();
		}

	}

	private void Explode()
	{
		RaycastHit[] hits = Physics.SphereCastAll(transform.position, soStats.ATTACK_RANGE_DEFAULT,
												  transform.up);

		foreach(var hit in hits)
		{
			if (CompareTag(hit.collider.tag))
			{
				continue;
			}

			IDamageable target = hit.collider.GetComponent<IDamageable>();
			if (target != null) 
			{
				target.TakenDamage(soStats.DAMAGE_DEFAULT);
			}
		}

		TakenDamage(soStats.HP_DEFAULT);
	}

	#endregion
}

