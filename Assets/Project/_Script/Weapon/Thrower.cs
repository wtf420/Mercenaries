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

        Type = GameConfig.WEAPON.GERNADETHROWER;
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
