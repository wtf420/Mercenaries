using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttack : IWeapon
{
    #region Fields & Properties
    [SerializeField] protected SO_WeaponGunStats soStats;

    [SerializeField] protected float _damage;
    [SerializeField] protected float _attackRadius;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _delayBetweenAttack;

    protected bool attackable = true;

    protected Character character;

    #endregion

    #region Methods
    public override void Initialize()
    {
        character = GetComponentInParent<Character>();

        Type = GameConfig.WEAPON.SWORD;
        // _damage = soStats.DAMAGE_DEFAULT;
        // _attackRange = soStats.ATTACK_RANGE_DEFAULT;
        // _attackRadius = soStats.ATTACK_RANGE_DEFAULT;
        // _delayBetweenAttack = soStats.ATTACK_SPEED_DEFAULT;
    }

    public override void AttemptAttack()
    {
        if (attackable)
        {
            StartCoroutine(Attack());
        }
    }

    public override void AttemptReload()
    {

    }

    protected virtual IEnumerator Attack()
    {
        attackable = false;
        int layermask = LayerMask.GetMask("Damageables");
        RaycastHit[] info = Physics.SphereCastAll(this.transform.position + transform.forward * _attackRange, _attackRadius, Vector3.up, 0, layermask);
        foreach (RaycastHit hit in info)
        {
            Debug.Log(hit.transform.gameObject);
            if (this.tag == hit.transform.tag || transform.parent.gameObject == hit.transform.gameObject)
            {
                Debug.Log("Skipped: ");
                continue;
            }

            //check if theres a wall between
            bool c = false;
            Vector3 hitlocation = (hit.point == Vector3.zero) ? hit.transform.position : hit.point;
            RaycastHit[] info2 = Physics.RaycastAll(this.transform.position, hitlocation, Vector3.Distance(this.transform.position, hit.transform.position));
            foreach (RaycastHit hit2 in info2)
            {
                //theres an object blocking
                if (hit2.collider.gameObject.GetComponent<BulletproofWall>() || (hit2.collider.gameObject.GetComponent<IDamageable>() == null))
                {
                    c = true;
                }
                Debug.Log("Blocked");
            }
            if (c) continue;

            if (hit.collider.gameObject.GetComponent<IDamageable>() != null)
            {
                Debug.Log("Damage");
                hit.collider.gameObject.GetComponent<IDamageable>().TakenDamage(new Damage(_damage, this.transform.position, DamageType.Melee, source));

            }
            Debug.DrawLine(this.transform.position, hitlocation, Color.green, 5f);
        }
        yield return new WaitForSeconds(_delayBetweenAttack);
        attackable = true;
    }

    protected IEnumerator IE_Reload()
    {
        yield return null;
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        // if (soStats != null)
        // {
        //     if (this.tag == "Player")
        //         Gizmos.color = Color.blue;
        //     else
        //         Gizmos.color = Color.red;
        //     Gizmos.DrawWireSphere(this.transform.position + transform.forward * soStats.ATTACK_RANGE_DEFAULT, soStats.ATTACK_RANGE_DEFAULT);
        // }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position + transform.forward * _attackRange, _attackRadius);
    }

    #endregion
}
