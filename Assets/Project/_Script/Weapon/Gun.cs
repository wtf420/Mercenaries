using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gun: IWeapon
{
	#region Fields & Properties
	[SerializeField] protected SO_WeaponGunStats soStats;
	[SerializeField] protected Bullet bulletPrefab;

	protected float _damage;
	protected float _attackRange;
	protected float _attackSpeed;
	protected float _reloadTime;
	protected int _magazineCapacity;
	protected float _inaccuracy;
	protected float _bulletSpeed;

	protected bool attackable = true;
	protected bool isReloading = false;
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

	void Start()
	{
		BulletChange?.Invoke((int)currentBulletQuantity);
	}

    public override void AttemptAttack()
    {
        if (currentBulletQuantity > 0 && attackable && !isReloading)
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

	protected virtual IEnumerator Attack()
	{
		attackable = false;
		// spawn bullet
		Vector3 direction = (transform.forward).normalized;
		Vector3 target = direction * 10f + Vector3.Cross(direction, transform.up).normalized * UnityEngine.Random.Range(-_inaccuracy, _inaccuracy);
		direction = (target).normalized;

		Bullet bullet = Instantiate(bulletPrefab, transform.position, new Quaternion());
		bullet.Initialize(_damage, _attackRange, _bulletSpeed, direction);
		bullet.tag = this.tag;
		bullet.source = this.source;

		gunSound.Stop();
		gunSound.Play();

		currentBulletQuantity -= 1;
		BulletChange?.Invoke((int)currentBulletQuantity);
		yield return new WaitForSeconds(delayBetweenShots);
		if (currentBulletQuantity == 0)
			StartCoroutine(IE_Reload());
		attackable = true;
	}

	protected IEnumerator IE_Reload()
	{
		isReloading = true;
		if (character)
			character.SetWorldText("Reloading...", _reloadTime);

		yield return new WaitForSeconds(_reloadTime);
		currentBulletQuantity = _magazineCapacity;
		BulletChange?.Invoke((int)currentBulletQuantity);
		isReloading = false;
	}

	public override int GetCurrentBullet => (int)currentBulletQuantity;

	[ExecuteInEditMode]
	private void OnDrawGizmos()
	{
		if (soStats != null)
		{
			if (this.tag == "Player")
				Gizmos.color = Color.blue;
			else
				Gizmos.color = Color.red;

			Vector3 direction = (transform.forward).normalized;
			Vector3 target1 = direction * soStats.ATTACK_RANGE_DEFAULT + Vector3.Cross(direction, transform.up).normalized * -soStats.INACCURACY;
			Vector3 target2 = direction * soStats.ATTACK_RANGE_DEFAULT + Vector3.Cross(direction, transform.up).normalized * soStats.INACCURACY;
			Gizmos.DrawLine(this.transform.position, this.transform.position + target1);
			Gizmos.DrawLine(this.transform.position, this.transform.position + target2);
		}
	}

	#endregion
}

