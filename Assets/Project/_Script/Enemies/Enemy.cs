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
	protected int currentPosition = 0;
	public Vector3 CurrentDestination { get; set; } = Vector3.zero;

	public bool IsInPatrolScope { get; set; }

	[Header("_~* 	Movement & control")]
	protected float _moveSpeed;
	protected float _HP;
	public bool HP { get {return HP; } }
	protected float _detectRange;
	protected float _attackRange;
	protected float _turningSpeed;

	protected bool _patrolable = true;
	protected internal bool isAlerted = false;

	protected Transform target;
	protected Healthbar healthbar;
	public UnityEvent<Enemy> OnDeathEvent;
	#endregion

	#region Methods
	public virtual void Initialize(Path p = null)
	{
		this.tag = GameConfig.COLLIDABLE_OBJECT.ENEMY.ToString();
		enemyAgent = GetComponent<NavMeshAgent>();
		characterRigidbody = GetComponent<Rigidbody>();
		OnDeathEvent = new UnityEvent<Enemy>();
		healthbar = GetComponentInChildren<Healthbar>();

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

		healthbar.Start();
	}

	void Start()
	{
		LevelManager.Instance.damageables.Add(this);
		CurrentDestination = path.GetNodePosition(currentPosition);
		enemyAgent.SetDestination(CurrentDestination);
	}

	public void Alert(GameObject? gameObject)
	{
		isAlerted = true;
		if (gameObject != null)
			target = gameObject.transform;
		else
			target = Character.Instance.transform;
	}

	public void AlertNearbyEnemies(GameObject? gameObject)
	{
		foreach (Collider c in Physics.OverlapSphere(this.transform.position, _detectRange, LayerMask.GetMask("Damageables")))
		{
			if (c.gameObject.CompareTag(this.tag) && c.gameObject != this.gameObject)
			{
				Enemy enemy = c.gameObject.GetComponent<Enemy>();
				if (enemy != null && !enemy.isAlerted)
					enemy.Alert(gameObject);
			}
		}
	}

	public virtual void UpdateEnemy()
	{
		if (!isAlerted)
		{
			target = DetectTarget();
			if (target != null)
				isAlerted = true;
		}
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

	public virtual void TakenDamage(Damage damage)
	{
		Alert(damage.damageSource);
		AlertNearbyEnemies(damage.damageSource);
		if (IsDead)
		{
			return;
		}

		if(_HP > 0)
		{
			_HP -= damage.value;
			healthbar.HealthUpdate();
			//Debug.Log($"Enemy hp: {_HP}");
			if (_HP <= 0)
			{
				IsDead = true;
				if (!deleteUponDeath)
				{
					if (damage.Origin != null)
					{
						Vector3 damageDirection = (Vector3)(damage.Origin - this.transform.position);
						
						enemyAgent.enabled = false;
						characterRigidbody.isKinematic = false;
						characterRigidbody.freezeRotation = false;
						characterRigidbody.AddForce(damageDirection.normalized * 5f, ForceMode.Impulse);
					}
				}
			}
		}
	}

	public virtual void OnDeath(Vector3? DamageDirection = null, float punch = 0.0f)
	{
		//Debug.Log("Enemy die");
		AlertNearbyEnemies(null);
		LevelManager.Instance.damageables.Remove(this);
		OnDeathEvent?.Invoke(this);
		OnDeathEvent?.RemoveAllListeners();
		if (deleteUponDeath)
		{
			float ratio = UnityEngine.Random.Range(0f, 1f);
			if (ratio < GameConfig.RATIO_DROP_BUFF)
			{
				Item.CreateBuff(transform.position, GameConfig.BUFF.HP, 10f);
			}
			else if (ratio < GameConfig.RATIO_DROP_ITEM)
			{
				Item.CreateItem(transform.position, weapon);
			}

			Destroy(gameObject);
		}
	}

	protected virtual Transform DetectTarget()
	{
		Transform target = null;
		float maxPriority = -9999;

		foreach (Collider c in Physics.OverlapSphere(this.transform.position, _detectRange, LayerMask.GetMask("Damageables")))
		{

			if (c.gameObject.CompareTag(this.tag)) continue;
			IDamageable damageable = c.gameObject.GetComponent<IDamageable>();
			if (damageable == null) continue;
			if (damageable.IsDead) continue;

			Transform damagableTransform = c.gameObject.transform;
			RaycastHit[] info = Physics.RaycastAll(this.transform.position, damagableTransform.position - this.transform.position, Vector3.Distance(this.transform.position, damagableTransform.position));
			bool blocked = false;
			foreach (RaycastHit hit in info)
			{
				//theres an object blocking
				if (hit.collider.gameObject.GetComponent<IDamageable>() == null)
				{
					blocked = true;
					break;
				}
			}
			if (blocked) continue;

			if (Vector3.Distance(damagableTransform.position, this.transform.position) <= _detectRange)
			{
				if (damageable.AttackPriority > maxPriority)
				{
					target = damagableTransform;
					maxPriority = damageable.AttackPriority;
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
		if(Vector3.Distance(transform.position, CurrentDestination) < 1f)
		{
			if (path.GetNode(currentPosition).GetType() == typeof(PatrolScope))
			{
				if (_patrolable)
				{
					StartCoroutine(IE_Patrol());
				}
			} 
			else
			{
				currentPosition++;
				if (currentPosition >= path.NodeCount())
					currentPosition = 0;
				CurrentDestination = path.GetNodePosition(currentPosition);
			}
		}
		enemyAgent.SetDestination(CurrentDestination);
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

	public virtual float GetHP()
	{
		return _HP;
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

