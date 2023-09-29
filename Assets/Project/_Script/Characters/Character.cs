using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	#region Fields & Properties
	[SerializeField] protected Rigidbody characterRigidbody;

	[SerializeField] Weapon weapon;

	public GameConfig.CHARACTER Type { get; protected set; }
	public Dictionary<GameConfig.STAT_TYPE, float> Stats { get; protected set; }
	public bool IsDeath { get; protected set; }
	public float acceleration, deAcceleration, drag, dashForce, dashTime;
	float speedX, speedZ, maxSpeed;

	bool movementEnable = true;

	//protected List<Weapon> weapons;
	#endregion

	#region Methods

	public virtual void Initialize()
	{
		characterRigidbody = GetComponent<Rigidbody>();

		SO_CharacterDefault stats = (SO_CharacterDefault)GameManager.Instance.GetStats(GameConfig.SO_TYPE.CHARACTER, (int)GameConfig.CHARACTER.CHARACTER_DEFAULT);

		Stats = new Dictionary<GameConfig.STAT_TYPE, float>();
		Stats.Add(GameConfig.STAT_TYPE.MOVE_SPEED, stats.MOVE_SPEED_DEFAULT);
		Stats.Add(GameConfig.STAT_TYPE.HP, stats.HP_DEFAULT);

		weapon.Initialize(transform);
		weapon.tag = this.tag;
		IsDeath = false;

		speedX = 0;
		speedZ = 0;
		maxSpeed = Stats[GameConfig.STAT_TYPE.MOVE_SPEED];
		//characterRigidbody.drag = drag;
	}

	public virtual void UpdateCharacter()
	{
		KeyboardController();
		MouseController();
	}

	public virtual void KeyboardController()
	{
		if (movementEnable)
		{
			Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
			movementInput = movementInput.normalized;

			if (Input.GetAxisRaw("Horizontal") != 0)
				speedX += acceleration * movementInput.x * Time.deltaTime;
			else
				speedX = Mathf.MoveTowards(speedX, 0, deAcceleration * Time.deltaTime);
			if (Input.GetAxisRaw("Vertical") != 0)
				speedZ += acceleration * movementInput.z * Time.deltaTime;
			else
				speedZ = Mathf.MoveTowards(speedZ, 0, deAcceleration * Time.deltaTime);
			speedX = Mathf.Clamp(speedX, -maxSpeed, maxSpeed);
			speedZ = Mathf.Clamp(speedZ, -maxSpeed, maxSpeed);
		} else
		{
			speedX = Mathf.MoveTowards(speedX, 0, deAcceleration * Time.deltaTime);
			speedZ = Mathf.MoveTowards(speedZ, 0, deAcceleration * Time.deltaTime);
		}
		Vector3 desiredMovement = new Vector3(speedX, 0, speedZ);

		Vector3 velocity = characterRigidbody.velocity;
		velocity.y = 0;
		float diffX = desiredMovement.x - velocity.x;
		float diffz = desiredMovement.z - velocity.z;
		//if theres no input or moving faster in same direction as input, no movement is apply, only friction
		diffX = (Input.GetAxisRaw("Horizontal") == 0 || desiredMovement.x / velocity.x > 0) ? Mathf.Clamp(diffX, -drag, drag) : Mathf.Clamp(diffX, -Mathf.Abs(speedX), Mathf.Abs(speedX));
		diffz = (Input.GetAxisRaw("Vertical") == 0 || desiredMovement.z / velocity.z > 0) ? Mathf.Clamp(diffz, -drag, drag) : Mathf.Clamp(diffz, -Mathf.Abs(speedZ), Mathf.Abs(speedZ));

		Vector3 movement = new Vector3(diffX, 0, diffz);
		characterRigidbody.velocity += movement;		

		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(Dash());
		}
	}

	//use this instead of Rigidbody.Addforce()
	void AddForce(Vector3 direction)
	{
		characterRigidbody.velocity = Vector3.zero;
		speedX = 0; speedZ = 0;
		characterRigidbody.AddForce(direction, ForceMode.Impulse);
	}

	IEnumerator Dash()
	{
		Vector3 dashMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		//Vector3 dashMovement = new Vector3(1, 0, 0);
		characterRigidbody.velocity = Vector3.zero;
		speedX = 0; speedZ = 0;
		characterRigidbody.AddForce(dashMovement.normalized * dashForce, ForceMode.Impulse);
		movementEnable = false;
		yield return new WaitForSeconds(dashTime);

		movementEnable = true;
		speedX = Mathf.Clamp(characterRigidbody.velocity.x, -maxSpeed, maxSpeed);
		speedZ = Mathf.Clamp(characterRigidbody.velocity.z, -maxSpeed, maxSpeed);
	}

	public void MouseController()
	{
		RotateWeapon();

		if(Input.GetMouseButton(0))
		{
			weapon.WeaponAttack();
		}
	}

	public void TakenDamage(float damage)
	{
		if (Stats[GameConfig.STAT_TYPE.HP] > 0)
		{
			Stats[GameConfig.STAT_TYPE.HP] -= damage;
			Debug.Log($"Character hp: {Stats[GameConfig.STAT_TYPE.HP]}");
			if (Stats[GameConfig.STAT_TYPE.HP] <= 0)
			{
				Debug.Log("Character die");
				IsDeath = true;
			}
		}
	}

	private void RotateWeapon()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitData;
		if (Physics.Raycast(ray, out hitData, 1000))
		{
			Vector3 direction = (hitData.point - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
			transform.rotation = lookRotation;
		}

		//characterRigidbody.velocity = Vector3.zero;
	}
	#endregion
}
