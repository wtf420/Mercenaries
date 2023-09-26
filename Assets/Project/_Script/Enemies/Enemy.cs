using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy: MonoBehaviour
{
	#region Fields & Properties
	[SerializeField] protected Rigidbody characterRigidbody;

	[SerializeField] Weapon weapon;

	//public CHARACTER_TYPE Type { get; protected set; }
	public Dictionary<GameConfig.STAT_TYPE, float> Stats { get; protected set; }

	//protected List<Weapon> weapons;
	#endregion

	#region Methods

	public virtual void Initialize()
	{
		this.tag = GameConfig.COLLIDABLE_OBJECT.ENEMY.ToString();

		characterRigidbody = GetComponent<Rigidbody>();

		SO_EnemyDefault stats = (SO_EnemyDefault)GameManager.Instance.GetStats(GameConfig.SO_TYPE.ENEMY, (int)GameConfig.ENEMY.ENEMY_DEFAULT);

		Stats = new Dictionary<GameConfig.STAT_TYPE, float>();
		Stats.Add(GameConfig.STAT_TYPE.MOVE_SPEED, stats.MOVE_SPEED_DEFAULT);
		Stats.Add(GameConfig.STAT_TYPE.HP, stats.HP_DEFAULT);

		weapon.Initialize(transform);
	}

	public virtual void UpdateEnemy(Character character)
	{
		weapon.WeaponAttack();
	}

	public virtual void TakenDamage(float damage)
	{
		if(Stats[GameConfig.STAT_TYPE.HP] > 0)
		{
			Stats[GameConfig.STAT_TYPE.HP] -= damage;
			Debug.Log($"Enemy hp: {Stats[GameConfig.STAT_TYPE.HP]}");
			if(Stats[GameConfig.STAT_TYPE.HP] <= 0)
			{
				Debug.Log("Enemy die");
				Destroy(gameObject);
			}
		}
	}
	#endregion
}

