using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class Gun: IWeapon
{
	#region Fields & Properties
	[SerializeField] protected SO_WeaponGunStats soStats;
	[SerializeField] Bullet bulletPrefab;

	protected float _damage;
	protected float _attackRange;
	protected float _attackSpeed;
	protected float _reloadTime;
	protected int _magazineCapacity;
	protected float _inaccuracy;
	protected float _bulletSpeed;

	protected bool attackable = true;
	protected float currentBulletQuantity;
	protected float delayBetweenShots;

	protected Character character;
	protected AudioSource gunSound;

	#endregion

	#region Methods
	public override void Initialize()
	{
		character = GetComponentInParent<Character>();
		
		Type = GameConfig.WEAPON.RIFLE;
		_damage = soStats.DAMAGE_DEFAULT;
		_attackRange = soStats.ATTACK_RANGE_DEFAULT;
		_attackSpeed = soStats.ATTACK_SPEED_DEFAULT;
		_bulletSpeed = soStats.BULLET_SPEED;
		_inaccuracy = soStats.INACCURACY;
		_magazineCapacity = soStats.MAGAZINE_CAPACITY;
		_reloadTime = soStats.RELOAD_TIME;

		currentBulletQuantity = _magazineCapacity;
		delayBetweenShots = 60f / _attackSpeed; //real guns use RPM (Rounds per minute) to calculate how fast they shoot
		gunSound = GetComponent<AudioSource>();
	}

    public override void AttemptAttack()
    {
        if (currentBulletQuantity > 0 && attackable)
		{
			StartCoroutine(Attack());
		} else if (currentBulletQuantity <= 0)
		{
			StartCoroutine(IE_Reload());
		}
    }

	public override void AttemptReload()
	{
		if (currentBulletQuantity < _magazineCapacity)
		{
			StartCoroutine(IE_Reload());
		}
	}

	protected IEnumerator Attack()
	{
		if (currentBulletQuantity > 0)
		{
			// spawn bullet
			Vector3 direction = (transform.forward).normalized;
			Vector3 target = direction * 10f + Vector3.Cross(direction, transform.up).normalized * UnityEngine.Random.Range(-_inaccuracy, _inaccuracy);
			direction = (target).normalized;

			Bullet bullet = Instantiate(bulletPrefab, transform.position, new Quaternion());
			bullet.Initialize(_damage, _attackRange, _bulletSpeed, direction);
			bullet.tag = this.tag;

			gunSound.Stop();
			gunSound.Play();

			currentBulletQuantity -= 1;
			attackable = false;
			yield return new WaitForSeconds(delayBetweenShots);
			if (currentBulletQuantity == 0)
				StartCoroutine(IE_Reload());
			attackable = true;
		}
	}

	protected IEnumerator IE_Reload()
	{
		attackable = false;
		character.SetWorldText("Reloading...");

		yield return new WaitForSeconds(_reloadTime);
		currentBulletQuantity = _magazineCapacity;
		character.SetWorldText("");
		attackable = true;
	}

	[ExecuteInEditMode]
	private void OnDrawGizmos()
	{
		if (soStats != null && Selection.Contains(gameObject))
		{
			float _inaccuracy = soStats.INACCURACY;

			Gizmos.color = Color.blue;
			Vector3 direction = (transform.forward).normalized;
			Vector3 target1 = direction * 10f + Vector3.Cross(direction, transform.up).normalized * -_inaccuracy;
			Vector3 target2 = direction * 10f + Vector3.Cross(direction, transform.up).normalized * _inaccuracy;
			Gizmos.DrawLine(this.transform.position, this.transform.position + target1);
			Gizmos.DrawLine(this.transform.position, this.transform.position + target2);
		}
	}

	#endregion
}

