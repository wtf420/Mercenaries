using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletproofWall : Weapon
{
	#region Fields & Properties
	float remainingTime = 3f;
	#endregion

	#region Methods
	public static BulletproofWall Create(Transform parent = null, Vector3? position = null, Quaternion? rotation = null)
	{
		BulletproofWall wall = Instantiate(Resources.Load<BulletproofWall>("_Prefabs/Weapon/BulletProofWall"), parent);
		wall.transform.rotation = (Quaternion)rotation;
		wall.transform.position =  (Vector3)position + wall.transform.forward * 2f;

		return wall;
	}

	public override void Initialize(Transform parent = null)
	{
		base.Initialize(parent);

		Type = GameConfig.WEAPON.BULLETPROOF_WALL;

		StartCoroutine(IE_RemainingTime());
	}

	protected override void Attack()
	{
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.CompareTag(other.tag))
		{
			return;
		}

		if (other.GetComponent<Bullet>() != null)
		{
			Destroy(other.gameObject);
		}
	}

	private IEnumerator IE_RemainingTime()
	{
		yield return new WaitForSeconds(remainingTime);

		Destroy(gameObject);
	}
	#endregion
}

