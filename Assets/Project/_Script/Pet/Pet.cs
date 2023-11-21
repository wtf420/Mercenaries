using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pet : MonoBehaviour, IDamagable
{
	#region Fields & Properties

	public GameConfig.PET Type { get; protected set; }
	public Dictionary<GameConfig.STAT_TYPE, float> Stats { get; protected set; }
	public bool IsDead { get; protected set; }
	public float AttackPriority { get; protected set; }

	protected Transform target;
	protected bool attackable;
	#endregion

	#region Methods
	public virtual void Initialize(string tag)
	{
		this.tag = tag;
		LevelManager.Instance.damagables.Add(this);
		Stats = new Dictionary<GameConfig.STAT_TYPE, float>();
		IsDead = false;
		attackable = true;

	}

	public virtual void UpdatePet(List<Enemy> enemies = null)
	{
		foreach(var enemy in enemies)
		{
			if(Vector3.Distance(enemy.transform.position, transform.position)
		    <= Stats[GameConfig.STAT_TYPE.ATTACK_RANGE])
			{
				target = enemy.transform;
				break;
			}
		}

		Attack();
	}

	public virtual void TakenDamage(float damage)
	{
		
	}

	protected virtual void Attack()
	{
		
	}

	protected virtual void OnDeath()
	{
		LevelManager.Instance.damagables.Remove(this);
	}

	protected IEnumerator IE_Reload()
	{
		yield return new WaitForSeconds(1.0f / Stats[GameConfig.STAT_TYPE.ATTACK_SPEED]);

		attackable = true;
	}
	#endregion
}

