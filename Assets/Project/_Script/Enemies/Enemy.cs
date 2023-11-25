using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy: MonoBehaviour, IDamagable
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

	[Header("_~* 	Movement & control")]
	protected float _moveSpeed;
	protected float _HP;
	protected float _detectRange;
	protected float _attackRange;
	protected float _turningSpeed;

	protected IDamagable target;
	#endregion

	#region Methods

	public virtual void Initialize(Path p = null)
	{
		this.tag = GameConfig.COLLIDABLE_OBJECT.ENEMY.ToString();
		LevelManager.Instance.damagables.Add(this);

		enemyAgent = GetComponent<NavMeshAgent>();
		characterRigidbody = GetComponent<Rigidbody>();

		//SO_EnemyDefault stats = (SO_EnemyDefault)LevelManager.Instance.GetStats(GameConfig.SO_TYPE.ENEMY, (int)GameConfig.ENEMY.ENEMY_DEFAULT);
		//SO_EnemyDefault stats = LevelManager.Instance.GetStats(this);
		_moveSpeed = soStats.MOVE_SPEED_DEFAULT;
		_HP = soStats.HP_DEFAULT;
		_detectRange = soStats.DETECT_RANGE;
		_attackRange = soStats.ATTACK_RANGE_DEFAULT;
		_turningSpeed = soStats.TURNING_SPEED;

		enemyAgent.speed = _moveSpeed;
		if (p != null)
		{
			path = p;
		}

		weapon.Initialize();
		weapon.tag = this.tag;
	}

	public virtual void UpdateEnemy(Character character)
	{
		target = DetectTarget();
		if (target != null)
		{
			Transform targetTransform = (target as MonoBehaviour).transform;
			if (Vector3.Distance(transform.position, targetTransform.position) <= _attackRange)
			{
				//stop walking and start attacking.
				enemyAgent.SetDestination(transform.position);
				RotateWeapon(targetTransform.position);
				weapon.AttemptAttack();
			} else
			{
				enemyAgent.SetDestination(targetTransform.position);
				MovementBehaviour();
			}
			Debug.DrawLine(transform.position, targetTransform.position, Color.blue);
		} else
		{
			MovementBehaviour();
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
				OnDeath();
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
				OnDeath(DamageDirection, (float)punch);
			}
		}
	}

	protected virtual void OnDeath(Vector3? DamageDirection = null, float punch = 0.0f)
	{
		//Debug.Log("Enemy die");
		LevelManager.Instance.damagables.Remove(this);
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

	protected virtual IDamagable DetectTarget()
	{
		IDamagable target = null;
		float maxPriority = -9999;
		foreach (IDamagable damagable in LevelManager.Instance.damagables)
		{
			Transform damagableTransform = (damagable as MonoBehaviour).transform;
			if (Vector3.Distance(damagableTransform.position, this.transform.position) <= _detectRange)
			{
				if (damagableTransform.tag == this.tag)
					continue;

				if (damagableTransform.gameObject.GetComponent<IDamagable>().IsDead)
					continue;
				else
				{
					if (damagableTransform.gameObject.GetComponent<IDamagable>().AttackPriority > maxPriority)
					{
						target = damagableTransform.gameObject.GetComponent<IDamagable>();
						maxPriority = damagableTransform.gameObject.GetComponent<IDamagable>().AttackPriority;
					}
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

