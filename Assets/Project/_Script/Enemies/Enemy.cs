using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy: MonoBehaviour
{
	#region Fields & Properties
	[SerializeField] protected Rigidbody characterRigidbody;
	[SerializeField] protected Weapon weapon;

	[SerializeField] protected Path path;

	protected NavMeshAgent enemyAgent;

	private int currentPosition = 0;

	//public CHARACTER_TYPE Type { get; protected set; }
	public Dictionary<GameConfig.STAT_TYPE, float> Stats { get; protected set; }

	public bool IsDead { get; protected set; }
	//protected List<Weapon> weapons;
	#endregion

	#region Methods

	public virtual void Initialize()
	{
		this.tag = GameConfig.COLLIDABLE_OBJECT.ENEMY.ToString();

		enemyAgent = GetComponent<NavMeshAgent>();
		characterRigidbody = GetComponent<Rigidbody>();

		//SO_EnemyDefault stats = (SO_EnemyDefault)LevelManager.Instance.GetStats(GameConfig.SO_TYPE.ENEMY, (int)GameConfig.ENEMY.ENEMY_DEFAULT);
		SO_EnemyDefault stats = LevelManager.Instance.GetStats(this);
		Stats = new Dictionary<GameConfig.STAT_TYPE, float>();
		Stats.Add(GameConfig.STAT_TYPE.MOVE_SPEED, stats.MOVE_SPEED_DEFAULT);
		Stats.Add(GameConfig.STAT_TYPE.HP, stats.HP_DEFAULT);
		Stats.Add(GameConfig.STAT_TYPE.DETECT_RANGE, stats.DETECT_RANGE);

		enemyAgent.speed = Stats[GameConfig.STAT_TYPE.MOVE_SPEED];

		weapon.Initialize(transform);
	}

	public virtual void UpdateEnemy(Character character)
	{
		weapon.WeaponAttack();
		MovementBehaviour();
	}

	public virtual void TakenDamage(float damage)
	{
		if(IsDead)
		{
			return;
		}

		if(Stats[GameConfig.STAT_TYPE.HP] > 0)
		{
			Stats[GameConfig.STAT_TYPE.HP] -= damage;
			Debug.Log($"Enemy hp: {Stats[GameConfig.STAT_TYPE.HP]}");
			if(Stats[GameConfig.STAT_TYPE.HP] <= 0)
			{
				Debug.Log("Enemy die");
				IsDead = true;
			}
		}
	}

	private void MovementBehaviour()
	{
		if (enemyAgent == null)
		{
			return;
		}

		Vector3 destination = path.GetNodePosition(currentPosition);
		enemyAgent.SetDestination(destination);
		if(Vector3.Distance(transform.position, destination) < 1f)
		{
			currentPosition++;
			if (currentPosition >= path.NodeCount())
				currentPosition = 0;

			//Debug.Log(currentPosition);
		}
	}

	#endregion
}

