using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shotgun : Gun
{
    #region Fields & Properties

    [SerializeField] int _bulletPerShot;
    #endregion

    #region Methods
    public override void Initialize()
    {
        character = GetComponentInParent<Character>();

        Type = GameConfig.WEAPON.SHOTGUN;
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

    protected override IEnumerator Attack()
    {
        attackable = false;
        // spawn bullet
        Vector3 direction = (transform.forward).normalized;
        for (int i = 0; i < _bulletPerShot; i++)
        {
            Vector3 target = direction * 10f + Vector3.Cross(direction, transform.up).normalized * UnityEngine.Random.Range(-_inaccuracy, _inaccuracy);

            Bullet bullet = Instantiate(bulletPrefab, transform.position, new Quaternion());
            bullet.Initialize(_damage, _attackRange, _bulletSpeed, target.normalized);
            bullet.tag = this.tag;
        }

        gunSound.Stop();
        gunSound.Play();

        currentBulletQuantity -= 1;
        if (currentBulletQuantity == 0)
            StartCoroutine(IE_Reload()); else
            yield return new WaitForSeconds(delayBetweenShots);
        attackable = true;
    }
    #endregion
}

