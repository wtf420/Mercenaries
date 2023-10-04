using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : Weapon
{
    #region Fields & Properties
    [SerializeField] GameObject gernadePrefab;
    public Vector3 throwAngleOffset;

    #endregion

    #region Methods
    public override void Initialize(Transform parent = null)
    {
        base.Initialize(parent);

        SO_WeaponGunStats stats = (SO_WeaponGunStats)GameManager.Instance.GetStats(GameConfig.SO_TYPE.WEAPON, (int)GameConfig.WEAPON.GERNADETHROWER);
        Stats.Add(WEAPON_STAT_TYPE.DAMAGE, stats.DAMAGE_DEFAULT);
        Stats.Add(WEAPON_STAT_TYPE.ATTACK_RANGE, stats.ATTACK_RANGE_DEFAULT);
        Stats.Add(WEAPON_STAT_TYPE.ATTACK_SPEED, stats.ATTACK_SPEED_DEFAULT);
        Stats.Add(WEAPON_STAT_TYPE.SPEED, stats.SPEED_DEFAULT);
        Stats.Add(WEAPON_STAT_TYPE.RELOAD_TIME, stats.RELOAD_TIME);
        Stats.Add(WEAPON_STAT_TYPE.QUANTITY, stats.BULLET_QUANTITY);

        currentBulletQuantity = (int)Stats[WEAPON_STAT_TYPE.QUANTITY];
    }

    protected override void Attack()
    {
        base.Attack();

        if (currentBulletQuantity > 0)
        {
            // spawn bullet
            GameObject gernade = Instantiate(gernadePrefab, this.transform.position, new Quaternion());
            gernade.GetComponent<Rigidbody>().AddForce((transform.forward + throwAngleOffset).normalized * 20f, ForceMode.Impulse);
            Debug.Log(gernade.GetComponent<Rigidbody>().velocity);

            currentBulletQuantity -= 1;
            if (currentBulletQuantity == 0)
                StartCoroutine(IE_Reload());
        }
    }

    #endregion
}
