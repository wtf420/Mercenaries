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
	[SerializeField] protected CharacterController characterController;
	[SerializeField] protected Rigidbody characterRigidbody;


	public CHARACTER_TYPE Type { get; protected set; }

	public Dictionary<STAT_TYPE, float> Stats { get; protected set; }
	#endregion

	#region Methods
	
	public virtual void Intialize(SO_CharacterDefault stats)
	{
		characterController = GetComponent<CharacterController>();
		characterRigidbody = GetComponent<Rigidbody>();

		Stats = new Dictionary<STAT_TYPE, float>();
		Stats.Add(STAT_TYPE.MOVE_SPEED, stats.MOVE_SPEED_DEFAULT);
	}

	public virtual void UpdateCharacter()
	{
		KeyboardController();
		MouseController();
	}

	public virtual void KeyboardController()
	{
		Vector3 delta = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		characterController.Move(delta * Stats[STAT_TYPE.MOVE_SPEED] * Time.deltaTime);
	}

	public void MouseController()
	{

	}
	#endregion
}
