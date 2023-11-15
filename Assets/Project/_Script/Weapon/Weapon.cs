using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WEAPON_STAT_TYPE
{
	DAMAGE,
	SPEED,
	ATTACK_SPEED,
	ATTACK_RANGE,
	EFFECT_SCOPE,
	QUANTITY,
	RELOAD_TIME,
	REMAINING_TIME
}

public class Weapon : MonoBehaviour
{
	#region Fields & Properties
	[SerializeField] protected SO_WeaponGunStats soStats;

	public Dictionary<WEAPON_STAT_TYPE, float> Stats { get; protected set; }
	protected Character character;
	public GameConfig.WEAPON Type { get; protected set; }
	protected int currentBulletQuantity;
	private bool attackable;
	#endregion

	#region Methods
	public virtual void Initialize(Transform parent = null)
	{
		if (parent != null)
			transform.parent = parent;
		character = GetComponentInParent<Character>();
		Stats = new Dictionary<WEAPON_STAT_TYPE, float>();
		attackable = true;

		Stats.Add(WEAPON_STAT_TYPE.DAMAGE, soStats.DAMAGE_DEFAULT);
		Stats.Add(WEAPON_STAT_TYPE.ATTACK_RANGE, soStats.ATTACK_RANGE_DEFAULT);
		Stats.Add(WEAPON_STAT_TYPE.ATTACK_SPEED, soStats.ATTACK_SPEED_DEFAULT);
		Stats.Add(WEAPON_STAT_TYPE.SPEED, soStats.SPEED_DEFAULT);
		Stats.Add(WEAPON_STAT_TYPE.RELOAD_TIME, soStats.RELOAD_TIME);
		Stats.Add(WEAPON_STAT_TYPE.QUANTITY, soStats.BULLET_QUANTITY);

	}	

	public void WeaponAttack()
	{
		if(attackable)
		{
			Attack();
			StartCoroutine(IE_CompleteAttack());
		}
	}

	protected virtual void Attack()
	{

	}

	protected IEnumerator IE_Reload()
	{
		character.SetWorldText("Reloading...");
		yield return new WaitForSeconds(Stats[WEAPON_STAT_TYPE.RELOAD_TIME]);
		currentBulletQuantity = (int)Stats[WEAPON_STAT_TYPE.QUANTITY];
		character.SetWorldText("");
	}

	IEnumerator IE_CompleteAttack()
	{
		attackable = false;

		yield return new WaitForSeconds(1f / Stats[WEAPON_STAT_TYPE.ATTACK_SPEED]);

		attackable = true;
	}

	#endregion
}

