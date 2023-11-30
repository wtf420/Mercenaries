using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SuicideAttacker : Enemy
{
	#region Fields & Properties

	public float _damageDefault;

	#endregion

	#region Methods

	public override void Initialize(Path p = null)
	{
		base.Initialize(p);
	}

	public override void UpdateEnemy()
	{
		Debug.LogWarning(target);
		target = DetectTarget();
		if (target != null)
		{
			Transform targetTransform = (target as MonoBehaviour).transform;
			if (Vector3.Distance(transform.position, targetTransform.position)
			< _attackRange)
			{
				Explode();

				return;
			} else
			{
				enemyAgent.SetDestination(targetTransform.position);
				MovementBehaviour();
			}

			return;
		} else
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

			IDamageable target = hit.collider.gameObject.GetComponent<IDamageable>();
			if (target != null) 
			{
				target.TakenDamage(_damageDefault);
			}
		}

		TakenDamage(soStats.HP_DEFAULT);
	}

	#endregion
}

