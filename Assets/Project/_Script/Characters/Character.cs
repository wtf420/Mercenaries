using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STAT_TYPE
{
	HP,
	ATTACK_SPEED,
	DAMAGE,
	MOVE_SPEED,
	ATTACK_RANGE,
	EFFECT_SCOPE,
}

public enum CHARACTER_TYPE
{
    CHARACTER_1,
    CHARACTER_2,
    CHARACTER_3,
    CHARACTER_4
}

public class Character : MonoBehaviour
{
	#region Fields & Properties
	[SerializeField] protected Rigidbody characterRigidbody;

	[SerializeField] Rifle rifle;

	public CHARACTER_TYPE Type { get; protected set; }
	public Dictionary<STAT_TYPE, float> Stats { get; protected set; }

	//protected List<Weapon> weapons;
	#endregion

	#region Methods
	
	public virtual void Intialize()
	{
		characterRigidbody = GetComponent<Rigidbody>();

		Stats = new Dictionary<STAT_TYPE, float>();
		Stats.Add(STAT_TYPE.MOVE_SPEED, ((SO_CharacterDefault)GameManager.Instance.CharacterStats[0]).MOVE_SPEED_DEFAULT);

		rifle.Initialize(transform);
	}

	public virtual void UpdateCharacter()
	{
		KeyboardController();
		MouseController();
	}

	public virtual void KeyboardController()
	{
		float horizontal = 0;
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

		transform.position += (delta.normalized * Stats[STAT_TYPE.MOVE_SPEED] * Time.deltaTime);
	}

	public void MouseController()
	{
		RotateWeapon();

		if(Input.GetMouseButton(0))
		{
			rifle.WeaponAttack();
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
