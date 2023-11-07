using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy: MonoBehaviour
{
	#region Fields & Properties
	[SerializeField] protected Rigidbody characterRigidbody;
	[SerializeField] protected Weapon weapon;
	[SerializeField] protected SO_EnemyDefault soStats;
	[SerializeField] protected Path path;
	[SerializeField] public bool deleteUponDeath = true;

	protected NavMeshAgent enemyAgent;

	private int currentPosition = 0;

	//public CHARACTER_TYPE Type { get; protected set; }
	public Dictionary<GameConfig.STAT_TYPE, float> Stats { get; protected set; }

	public bool IsDead { get; protected set; }
	//protected List<Weapon> weapons;

	protected Transform target;
	#endregion

	#region Methods

	public virtual void Initialize(Path p = null)
	{
		this.tag = GameConfig.COLLIDABLE_OBJECT.ENEMY.ToString();

		enemyAgent = GetComponent<NavMeshAgent>();
		characterRigidbody = GetComponent<Rigidbody>();

		//SO_EnemyDefault stats = (SO_EnemyDefault)LevelManager.Instance.GetStats(GameConfig.SO_TYPE.ENEMY, (int)GameConfig.ENEMY.ENEMY_DEFAULT);
		//SO_EnemyDefault stats = LevelManager.Instance.GetStats(this);
		Stats = new Dictionary<GameConfig.STAT_TYPE, float>();
		Stats.Add(GameConfig.STAT_TYPE.MOVE_SPEED, soStats.MOVE_SPEED_DEFAULT);
		Stats.Add(GameConfig.STAT_TYPE.HP, soStats.HP_DEFAULT);
		Stats.Add(GameConfig.STAT_TYPE.DETECT_RANGE, soStats.DETECT_RANGE);
		Stats.Add(GameConfig.STAT_TYPE.ATTACK_RANGE, soStats.ATTACK_RANGE_DEFAULT);

		enemyAgent.speed = Stats[GameConfig.STAT_TYPE.MOVE_SPEED];
		if (p != null)
		{
			path = p;
		}

		weapon.Initialize(transform);
		weapon.tag = this.tag;
	}

	public virtual void UpdateEnemy(Character character)
	{
		if(target)
		{
			if (Vector3.Distance(transform.position, target.position)
			<= Stats[GameConfig.STAT_TYPE.ATTACK_RANGE])
			{
				//stop walking and start attacking.
				enemyAgent.SetDestination(transform.position);

				RotateWeapon(target.position);
				weapon.WeaponAttack();

				return;
			}

			FindTarget();

			return;
		}

		if(!IsDetectSuccessful(character))
		{
			MovementBehaviour();
		}

	}

	public virtual void TakenDamage(float Damage, Vector3? DamageDirection = null, float punch = 0.0f)
	{
		if(IsDead)
		{
			return;
		}

		if(Stats[GameConfig.STAT_TYPE.HP] > 0)
		{
			Stats[GameConfig.STAT_TYPE.HP] -= Damage;
			Debug.Log($"Enemy hp: {Stats[GameConfig.STAT_TYPE.HP]}");
			if(Stats[GameConfig.STAT_TYPE.HP] <= 0)
			{
				OnDeath(DamageDirection, punch);
			}
		}
	}

	public virtual void OnDeath(Vector3? DamageDirection = null, float punch = 0.0f)
	{
		Debug.Log("Enemy die");
		IsDead = true;
		if (DamageDirection != null)
		{
			Vector3 damageDirection = (Vector3)DamageDirection;

			enemyAgent.enabled = false;
			characterRigidbody.isKinematic = false;
			characterRigidbody.freezeRotation = false;
			characterRigidbody.AddForce(damageDirection.normalized * punch, ForceMode.Impulse);
		}
	}

	private bool IsDetectSuccessful(Character character)
	{
		if (character.myPet)
		{
			if (Vector3.Distance(transform.position, character.myPet.transform.position)
			<= Stats[GameConfig.STAT_TYPE.DETECT_RANGE])
			{
				target = character.myPet.transform;

				return true;
			}
		}

		if (Vector3.Distance(transform.position, character.transform.position)
			<= Stats[GameConfig.STAT_TYPE.DETECT_RANGE])
		{
			RotateWeapon(character.transform.position);
			weapon.WeaponAttack();
			target = character.transform;

			return true;
		}

		return false;
	}

	private void RotateWeapon(Vector3 location)
	{
		var q = Quaternion.LookRotation(location - transform.position);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 1000f * Time.deltaTime);
	}

	private void FindTarget()
	{
		enemyAgent.SetDestination(target.transform.position);
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

