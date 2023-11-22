using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Thrower : IWeapon
{
    #region Fields & Properties
    [SerializeField] SO_WeaponThrowerStats soStats;
    [SerializeField] GameObject gernadePrefab;
    public float throwAngleOffset = 30; //in degrees

    protected float _damage;
    protected float _attackRange;
    protected float _attackSpeed;
    protected float _cooldown;
    protected float _throwforce;
    protected int _maxGernadeCount;

    protected float cooldownTimer = 0f;
    protected bool attackable = true;
    protected int currentGernadeCount;
    protected float delayBetweenThrow;

    #endregion

    #region Methods
    public override void Initialize()
    {
        Type = GameConfig.WEAPON.GERNADETHROWER;
        _damage = soStats.DAMAGE_DEFAULT;
        _attackRange = soStats.ATTACK_RANGE_DEFAULT;
        _attackSpeed = soStats.ATTACK_SPEED_DEFAULT;
        _cooldown = soStats.COOLDOWN;
        _throwforce = soStats.THROWFORCE;
        _maxGernadeCount = soStats.MAX_GERNADE_COUNT;

        delayBetweenThrow = 60f / _attackSpeed;
        currentGernadeCount = _maxGernadeCount;
    }

    public override void AttemptAttack()
    {
        if (currentGernadeCount > 0 && attackable)
            StartCoroutine(Attack());
    }

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        } else
        if (currentGernadeCount < _maxGernadeCount)
        {
            currentGernadeCount++;
            cooldownTimer = _cooldown;
        }
    }

    protected IEnumerator Attack()
    {
        attackable = false;
        currentGernadeCount--;
        GameObject gernade = Instantiate(gernadePrefab, this.transform.position, new Quaternion());
        gernade.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Throw(Character.Instance.GetWorldMousePosition(), gernade);
        //gernade.GetComponent<Rigidbody>().AddForce((transform.forward + throwAngleOffset).normalized * _throwforce, ForceMode.Impulse);
        yield return new WaitForSeconds(delayBetweenThrow);
        attackable = true;
    }

    void Throw(Vector3 target, GameObject gernade)
    {
        Vector3 direction = target - gernade.transform.position;
        float h = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float a = throwAngleOffset * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);

        // calculate velocity
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        gernade.GetComponent<Rigidbody>().AddForce(velocity * direction.normalized, ForceMode.Impulse);
    }

    #endregion
}
