using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DicWeapon
{
	[SerializeField] public GameConfig.WEAPON _Type;
	[SerializeField] public IWeapon _Weapon;
}

public class Character : MonoBehaviour, IDamageable
{
	public static Character Instance { get; protected set; }

	#region Fields & Properties
	[Header("_~* 	Prefabs, Weapons & Stats")]
	[SerializeField] protected List<DicWeapon> weapons;
	[SerializeField] protected SO_CharacterDefault soStats;
	[SerializeField] protected bool invulnerable = false;

	[Header("_~* 	User Interface")]
	[SerializeField] protected Canvas worldCanvas;
	[SerializeField] protected Canvas screenCanvas;

	[SerializeField] protected Slider healthbar;
	[SerializeField] protected TextMeshProUGUI worldText;
	[SerializeField] protected TextMeshProUGUI screenText;

	[Header("_~* 	Movement & control")]
	[SerializeField] protected Rigidbody characterRigidbody;
	[SerializeField] bool alignWithCamera = true;
	[SerializeField] float acceleration = 50f, deAcceleration = 50f, drag, dashForce = 50f, dashTime = 0.1f;

	int currentWeapon = 0;

	public float AttackPriority { get; protected set; }
	public Pet myPet { get; protected set; }
	public GameConfig.CHARACTER Type { get; protected set; }
	public bool IsDead { get; protected set; }

	protected float _HP;
	protected float _damage;
	protected float _moveSpeed;
	protected float _skillCooldown;

	float speedX, speedZ, maxSpeed;
	protected private Vector3 mousePos;
	bool movementEnable = true;

	#endregion

	#region Methods
	public static Character Create(Transform parent, Vector3 position)
	{
		Character character = Instantiate(Resources.Load<Character>("_Prefabs/Characters/Character"), parent);
		character.transform.position = position;
		
		return character;
	}

	public virtual void Initialize()
	{
		Instance = this;
		LevelManager.Instance.damagables.Add(this);
		characterRigidbody = GetComponent<Rigidbody>();

		//SO_Stats = GameManager.Instance.DataBank.weaponStats;
		//SO_CharacterDefault stats = GameManager.Instance.selectedCharacter.characterStats;

		_moveSpeed = soStats.MOVE_SPEED_DEFAULT;
		_HP = soStats.HP_DEFAULT;
		_skillCooldown = soStats.SKILL_COOLDOWN;

		foreach (IWeapon w in GetComponentsInChildren<IWeapon>())
		{
			DicWeapon dw = new DicWeapon();
			dw._Type = w.Type;
			dw._Weapon = w;
			weapons.Add(dw);
		}
		foreach (var weapon in weapons)
		{
			weapon._Weapon.Initialize();
			weapon._Weapon.tag = this.tag;
		}
		healthbar.minValue = 0;
		healthbar.maxValue = _HP;
		healthbar.value = _HP;

		IsDead = false;

		speedX = 0;
		speedZ = 0;
		maxSpeed = _moveSpeed;
		//characterRigidbody.drag = drag;
	}

	public virtual void UpdateUI()
	{
		healthbar.value = _HP;
		worldCanvas.transform.LookAt(transform.position + Camera.main.transform.forward);
	}

	public virtual void UpdateCharacter(List<Enemy> enemies = null)
	{
		KeyboardController();
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(Dash());
		}
		MouseController();
		UpdateUI();
	}

	public virtual void KeyboardController()
	{
		CharacterMovement();

		SwapWeapon();
	}

	public void MouseController()
	{
		mousePos = GetWorldMousePosition();
		RotateWeapon();

		if (Input.GetMouseButton(0))
		{
			weapons[currentWeapon]._Weapon.AttemptAttack();
		}
	}

	public void TakenDamage(float damage)
	{
		if (_HP > 0 && !invulnerable)
		{
			_HP -= damage;
			healthbar.value = _HP;
			Debug.Log($"Character hp: {_HP}");
			if (_HP <= 0)
			{
				Debug.Log("Character die");
				OnDeath();
			}
		}
	}

	private void CharacterMovement()
	{
		Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		Vector3 movementInputAligned = movementInput.normalized;
		if (alignWithCamera)
		{
			Vector3 camToPlayer = this.transform.position - Camera.main.transform.position;
			camToPlayer.y = 0;
			movementInputAligned = Quaternion.LookRotation(camToPlayer.normalized) * movementInputAligned;
		}

		if (movementEnable)
		{
			if (Input.GetAxisRaw("Horizontal") != 0)
			{   //Use deAcceleration when changing moving direction (currently disabled)
				// if (speedX / Input.GetAxisRaw("Horizontal") > 0)
				// 	speedX += acceleration * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
				// else
				// 	speedX += deAcceleration * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
				speedX += acceleration * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
				speedX = Mathf.Clamp(speedX, -maxSpeed * Mathf.Abs(movementInput.x), maxSpeed * Mathf.Abs(movementInput.x));
			}
			else
				speedX = Mathf.MoveTowards(speedX, 0, deAcceleration * Time.deltaTime);
			if (Input.GetAxisRaw("Vertical") != 0)
			{   //Use deAcceleration when changing moving direction (currently disabled)
				// if (speedX / Input.GetAxisRaw("Horizontal") > 0)
				// 	speedZ += acceleration * Input.GetAxisRaw("Vertical") * Time.deltaTime;
				// else
				// 	speedZ += deAcceleration * Input.GetAxisRaw("Vertical") * Time.deltaTime;
				speedZ += acceleration * Input.GetAxisRaw("Vertical") * Time.deltaTime;
				speedZ = Mathf.Clamp(speedZ, -maxSpeed * Mathf.Abs(movementInput.z), maxSpeed * Mathf.Abs(movementInput.z));
			}
			else
				speedZ = Mathf.MoveTowards(speedZ, 0, deAcceleration * Time.deltaTime);
		}
		else
		{
			speedX = Mathf.MoveTowards(speedX, 0, deAcceleration * Time.deltaTime);
			speedZ = Mathf.MoveTowards(speedZ, 0, deAcceleration * Time.deltaTime);
		}

		Vector3 desiredMovement = new Vector3(speedX, 0, speedZ);
		if (alignWithCamera)
		{
			Vector3 camToPlayer = this.transform.position - Camera.main.transform.position;
			camToPlayer.y = 0;
			desiredMovement = Quaternion.LookRotation(camToPlayer.normalized) * desiredMovement;
		}
		//Debug.Log(desiredMovement + ", " + Input.GetAxisRaw("Horizontal") + ", " + Input.GetAxisRaw("Vertical"));
		
		Vector3 velocity = characterRigidbody.velocity;
		velocity.y = 0;
		float diffX = desiredMovement.x - velocity.x;
		float diffz = desiredMovement.z - velocity.z;

		//if theres no input or moving faster in same direction as input, no movement is apply, only friction
		diffX = Mathf.Clamp(diffX, -Mathf.Abs(desiredMovement.x), Mathf.Abs(desiredMovement.x));
		diffz = Mathf.Clamp(diffz, -Mathf.Abs(desiredMovement.z), Mathf.Abs(desiredMovement.z));
		
		Vector3 movement = new Vector3(diffX, 0, diffz);
		
		characterRigidbody.velocity += movement;

		//Debug.Log("Velocity: " + characterRigidbody.velocity + " | desiredMovement : " + desiredMovement + " | movement: " + movement);
	}

	public virtual void OnDeath()
	{
		LevelManager.Instance.damagables.Remove(this);
		StopAllCoroutines();
		IsDead = true;
	}

	public virtual void SwapWeapon()
	{
		// Main Weapon
		if(Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
		{
			// Maybe _Type can be change in the future, but Keypad1 will be the main Weapon of Character.
			int index = 0;//weapons.FindIndex(weapon => weapon._Type == GameConfig.WEAPON.RIFLE);
			if(index != -1)
			{
				currentWeapon = index;
			}
			Debug.Log("Change to main weapon");
		}

		// Grenade
		if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
		{
			// Maybe _Type can be change in the future, but Keypad1 will be the main Weapon of Character.
			//int index = 1;
			//if (index != -1)
			//{
			//	currentWeapon = index;
			//}
			currentWeapon = 1;
			Debug.Log("Change to second weapon");
		}
	}

	public void AddForce(Vector3 direction)
	{
		//characterRigidbody.velocity = Vector3.zero;
		speedX = 0; speedZ = 0;
		characterRigidbody.AddForce(direction, ForceMode.Impulse);
	}

	IEnumerator Dash()
	{
		Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		if (alignWithCamera)
		{
			Vector3 camToPlayer = this.transform.position - Camera.main.transform.position;
			camToPlayer.y = 0;
			movementInput = Quaternion.LookRotation(camToPlayer) * movementInput;
		}
		movementInput = movementInput.normalized;

		characterRigidbody.velocity = Vector3.zero;
		speedX = 0; speedZ = 0;
		characterRigidbody.AddForce(movementInput.normalized * dashForce, ForceMode.Impulse);
		movementEnable = false;
		yield return new WaitForSeconds(dashTime);
		movementEnable = true;
	}

	private void RotateWeapon()
	{
		Vector3 location = mousePos;
		var q = Quaternion.LookRotation(location - transform.position);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 1000f * Time.deltaTime);
	}

	public Vector3 GetWorldMousePosition()
	{
		Plane plane = new Plane(Vector3.up, transform.position);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		plane.Raycast(ray, out float distance);
		return ray.GetPoint(distance);
	}

	public void SetWorldText(string t)
	{
		worldText.SetText(t);
	}

	public void SetScreenText(string t)
	{
		screenText.SetText(t);
	}

	[ExecuteInEditMode]
	private void ApplyChanges()
	{
		foreach (IWeapon w in GetComponentsInChildren<IWeapon>())
		{
			DicWeapon dw = new DicWeapon();
			dw._Type = w.Type;
			dw._Weapon = w;
			weapons.Add(dw);
		}
	}
	#endregion
}
