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
	public float acceleration;

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
	}

	public virtual void UpdateCharacter()
	{
		KeyboardController();
		//MouseController();
	}

	public virtual void KeyboardController()
	{
		/*float horizontal = 0;
		if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
		{
			horizontal = 1;
		}

		float vertical = 0;
		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
		{
			vertical = 1;
		}
		Vector3 delta = new Vector3(Input.GetAxis("Horizontal") * horizontal, 0, Input.GetAxis("Vertical") * vertical);

		transform.position += (delta.normalized * Stats[GameConfig.STAT_TYPE.MOVE_SPEED] * Time.deltaTime);*/

		//CHUA CODE XONG, TAM DE DAY
		if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
		{
			Vector3 MovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			Vector3 desiredMovement = MovementInput.normalized * Stats[GameConfig.STAT_TYPE.MOVE_SPEED];
			Vector3 diff = desiredMovement - characterRigidbody.velocity;
			Vector3 diff2 = diff.normalized * acceleration;

			Vector3 movement;
			if (Mathf.Abs(diff.magnitude) > Mathf.Abs(diff2.magnitude))
			{
				movement = diff2;
			} else
			{
				movement = diff;
			}

			movement.y = 0;
			characterRigidbody.velocity += movement;

			Debug.Log("Velocity: " + characterRigidbody.velocity
						+ " | desiredMovement : " + desiredMovement
						+ " | movement: " + movement);
		}
		

		if (Input.GetKeyDown(KeyCode.Space))
		{
			Vector3 desiredMovement = new Vector3(0,1,0);
			characterRigidbody.AddForce(desiredMovement.normalized * 10.0f, ForceMode.Impulse);
			Debug.Log("DASH velocity:" + characterRigidbody.velocity);
		}

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

		characterRigidbody.velocity = Vector3.zero;
	}
	#endregion
}
