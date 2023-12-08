using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy: MonoBehaviour, IDamageable
{
	#region Fields & Properties
	[Header("_~* 	Prefabs, Weapons & Stats")]
	[SerializeField] protected Rigidbody characterRigidbody;
	[SerializeField] protected IWeapon weapon;
	[SerializeField] protected SO_EnemyDefault soStats;
	[SerializeField] protected Path path;
	[SerializeField] public bool deleteUponDeath = true;
	public bool IsDead { get; protected set; }
	public float AttackPriority { get; protected set; }

	protected NavMeshAgent enemyAgent;
	private int currentPosition = 0;
	public Vector3 CurrentDestination { get; set; } = Vector3.zero;

	public bool IsInPatrolScope { get; set; }

	[Header("_~* 	Movement & control")]
	protected float _moveSpeed;
	protected float _HP;
	protected float _detectRange;
	protected float _attackRange;
	protected float _turningSpeed;

	protected bool _patrolable = true;

	protected Transform target;
	#endregion

	#region Methods
	public UnityEvent<Enemy> OnDeathEvent;

	public virtual void Initialize(Path p = null)
	{
		this.tag = GameConfig.COLLIDABLE_OBJECT.ENEMY.ToString();
		enemyAgent = GetComponent<NavMeshAgent>();
		characterRigidbody = GetComponent<Rigidbody>();
		OnDeathEvent = new UnityEvent<Enemy>();

		//SO_EnemyDefault stats = (SO_EnemyDefault)LevelManager.Instance.GetStats(GameConfig.SO_TYPE.ENEMY, (int)GameConfig.ENEMY.ENEMY_DEFAULT);
		//SO_EnemyDefault stats = LevelManager.Instance.GetStats(this);
		_moveSpeed = soStats.MOVE_SPEED_DEFAULT;
		_HP = soStats.HP_DEFAULT;
		_detectRange = soStats.DETECT_RANGE;
		_attackRange = soStats.ATTACK_RANGE_DEFAULT;
		_turningSpeed = soStats.TURNING_SPEED;

		enemyAgent.speed = _moveSpeed;
		enemyAgent.angularSpeed = _turningSpeed;
		enemyAgent.acceleration = _moveSpeed;

		enemyAgent.speed = _moveSpeed;
		if (p != null)
		{
			path = p;
		}

		weapon = GetComponentInChildren<IWeapon>();
		if (weapon)
		{
			weapon.Initialize();
			weapon.tag = this.tag;
		}
	}

	void Start()
	{
		LevelManager.Instance.damageables.Add(this);
		CurrentDestination = path.GetNodePosition(currentPosition);
	}

	public virtual void UpdateEnemy()
	{
		target = DetectTarget();
		if (target != null)
		{
			if (Vector3.Distance(transform.position, target.position) <= _attackRange)
			{
				//stop walking and start attacking.
				enemyAgent.SetDestination(transform.position);
				RotateWeapon(target.position);
				weapon.AttemptAttack();
			} else
			{
				enemyAgent.SetDestination(target.position);
				RotateWeapon(target.position);
			}
			Debug.DrawLine(transform.position, target.position, Color.blue);
		} 
		else
		{
			MovementBehaviour();
			Debug.DrawLine(transform.position, CurrentDestination, Color.blue);
		}
	}

	public virtual void TakenDamage(float Damage)
	{
		if (IsDead)
		{
			return;
		}

		if (_HP > 0)
		{
			_HP -= Damage;
			//Debug.Log($"Enemy hp: {_HP}");
			if (_HP <= 0)
			{
				IsDead = true;
			}
		}
	}

	public virtual void TakenDamage(float Damage, Vector3? DamageDirection = null, float? punch = 0.0f)
	{
		if(IsDead)
		{
			return;
		}

		if(_HP > 0)
		{
			_HP -= Damage;
			//Debug.Log($"Enemy hp: {_HP}");
			if(_HP <= 0)
			{
				IsDead = true;
			}
		}
	}

	public virtual void OnDeath(Vector3? DamageDirection = null, float punch = 0.0f)
	{
		//Debug.Log("Enemy die");
		LevelManager.Instance.damageables.Remove(this);
		OnDeathEvent?.Invoke(this);
		OnDeathEvent?.RemoveAllListeners();
		if (!deleteUponDeath)
		{
			if (DamageDirection != null)
			{
				Vector3 damageDirection = (Vector3)DamageDirection;

				enemyAgent.enabled = false;
				characterRigidbody.isKinematic = false;
				characterRigidbody.freezeRotation = false;
				characterRigidbody.AddForce(damageDirection.normalized * punch, ForceMode.Impulse);
			}
		} else
		{
			Destroy(gameObject);
		}
	}

	protected virtual Transform DetectTarget()
	{
		Transform target = null;
		float maxPriority = -9999;
		foreach (IDamageable damagable in LevelManager.Instance.damageables)
		{
			if (damagable.IsDead)
				continue;

			Transform damagableTransform = (damagable as MonoBehaviour).transform;
			if (damagableTransform.tag == this.tag)
				continue;

			if (Vector3.Distance(damagableTransform.position, this.transform.position) <= _detectRange)
			{
				if (damagableTransform.gameObject.GetComponent<IDamageable>().AttackPriority > maxPriority)
				{
					target = damagableTransform;
					maxPriority = damagableTransform.gameObject.GetComponent<IDamageable>().AttackPriority;
				}

			}
		}

		return target;
	}

	protected void RotateWeapon(Vector3 location)
	{
		var q = Quaternion.LookRotation(location - transform.position);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, q, _turningSpeed * Time.deltaTime);
	}

	protected virtual void MovementBehaviour()
	{
		// if (enemyAgent == null)
		// {
		// 	return;
		// }

		// if (!_patrolable)
		// {
		// 	return;
		// }

		// if(CurrentDestination == Vector3.zero)
		// {
		// 	CurrentDestination = patrolScope.GetRandomDestination(transform.position);
		// }

		// enemyAgent.SetDestination(CurrentDestination);

		// if (target != null)
		// {
		// 	if (Vector3.Distance(target.position, transform.position) <= _detectRange)
		// 	{
		// 		enemyAgent.SetDestination(transform.position);
		// 	}
		// 	else if(target.GetComponent<IDamageable>().IsInPatrolScope)
		// 	{
		// 		enemyAgent.SetDestination(target.transform.position);
		// 	}

		// 	return;
		// }

		// if(gameObject.GetComponent<SuicideAttacker>())
		// 	Debug.Log(Vector3.Distance(enemyAgent.destination, CurrentDestination));

		// if (Vector3.Distance(transform.position, CurrentDestination) < 1f)
		// {
		// 	StartCoroutine(IE_Patrol());
		// 	CurrentDestination = patrolScope.GetRandomDestination(enemyAgent.destination);
		// }

		//Debug.Log("Node: " + currentPosition + " - Position: " + CurrentDestination);
		enemyAgent.SetDestination(CurrentDestination);
		if(Vector3.Distance(transform.position, CurrentDestination) < 1f)
		{
			if (path.GetNode(currentPosition).GetType() == typeof(PatrolScope))
			{
				if (_patrolable)
				{
					StartCoroutine(IE_Patrol());
				}
			} else
			{
				currentPosition++;
				if (currentPosition >= path.NodeCount())
					currentPosition = 0;
				CurrentDestination = path.GetNodePosition(currentPosition);
			}
		}
	}

	protected IEnumerator IE_Patrol()
	{
		_patrolable = false;
		yield return new WaitForSeconds(GameConfig.TIME_STOP_AFTER_PATROLLING);
		_patrolable = true;

		currentPosition++;
		if (currentPosition >= path.NodeCount())
			currentPosition = 0;
		CurrentDestination = path.GetNodePosition(currentPosition);
	}

	protected virtual IEnumerator Skill()
	{
		yield return null;
	}

	[ExecuteInEditMode]
	private void OnDrawGizmos()
	{
		if (soStats != null && Selection.Contains(gameObject))
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(transform.position, soStats.DETECT_RANGE);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, soStats.ATTACK_RANGE_DEFAULT);
		}
	}
	#endregion
}

