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
	RELOAD_TIME
}

public class Weapon : MonoBehaviour
{
	#region Fields & Properties

	public Dictionary<WEAPON_STAT_TYPE, float> Stats { get; protected set; }

	public GameConfig.WEAPON Type { get; protected set; }
	protected int currentBulletQuantity;
	private bool attackable;
	#endregion

	#region Methods
	public virtual void Initialize(Transform parent = null, SO_WeaponGunStats gunStats = null)
	{
		transform.parent = parent;
		Stats = new Dictionary<WEAPON_STAT_TYPE, float>();
		attackable = true;

		if (gunStats == null)
		{
			SO_WeaponGunStats stats = (SO_WeaponGunStats)GameManager.Instance.GetStats(this);
			Stats.Add(WEAPON_STAT_TYPE.DAMAGE, stats.DAMAGE_DEFAULT);
			Stats.Add(WEAPON_STAT_TYPE.ATTACK_RANGE, stats.ATTACK_RANGE_DEFAULT);
			Stats.Add(WEAPON_STAT_TYPE.ATTACK_SPEED, stats.ATTACK_SPEED_DEFAULT);
			Stats.Add(WEAPON_STAT_TYPE.SPEED, stats.SPEED_DEFAULT);
			Stats.Add(WEAPON_STAT_TYPE.RELOAD_TIME, stats.RELOAD_TIME);
			Stats.Add(WEAPON_STAT_TYPE.QUANTITY, stats.BULLET_QUANTITY);
		} 
		else
		{
			Stats.Add(WEAPON_STAT_TYPE.DAMAGE, gunStats.DAMAGE_DEFAULT);
			Stats.Add(WEAPON_STAT_TYPE.ATTACK_RANGE, gunStats.ATTACK_RANGE_DEFAULT);
			Stats.Add(WEAPON_STAT_TYPE.ATTACK_SPEED, gunStats.ATTACK_SPEED_DEFAULT);
			Stats.Add(WEAPON_STAT_TYPE.SPEED, gunStats.SPEED_DEFAULT);
			Stats.Add(WEAPON_STAT_TYPE.RELOAD_TIME, gunStats.RELOAD_TIME);
			Stats.Add(WEAPON_STAT_TYPE.QUANTITY, gunStats.BULLET_QUANTITY);
		}
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
		yield return new WaitForSeconds(Stats[WEAPON_STAT_TYPE.RELOAD_TIME]);
		currentBulletQuantity = (int)Stats[WEAPON_STAT_TYPE.QUANTITY];
	}

	IEnumerator IE_CompleteAttack()
	{
		attackable = false;

		yield return new WaitForSeconds(1f / Stats[WEAPON_STAT_TYPE.ATTACK_SPEED]);

		attackable = true;
	}

	#endregion
}

