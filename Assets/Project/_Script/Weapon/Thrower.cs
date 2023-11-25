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
    protected float _delayBetweenThrow;
    protected float _cooldown;
    protected float _maxRange;
    protected int _maxGernadeCount;

    protected float cooldownTimer = 0f;
    protected bool attackable = true;
    protected int currentGernadeCount;

    #endregion

    #region Methods
    public override void Initialize()
    {
        Type = GameConfig.WEAPON.GERNADETHROWER;
        _damage = soStats.DAMAGE_DEFAULT;
        _delayBetweenThrow = soStats.DELAY_BETWEEN_THROW;
        _cooldown = soStats.COOLDOWN;
        _maxRange = soStats.MAX_RANGE;
        _maxGernadeCount = soStats.MAX_GERNADE_COUNT;

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
        gernade.tag = this.tag;
        gernade.GetComponent<Rigidbody>().velocity = Vector3.zero;
        StartCoroutine(IgnoreCollision(gernade.gameObject));

        Vector3 targetPosition;
        if (this.tag == "Player") targetPosition = Character.Instance.GetWorldMousePosition();
        else targetPosition = Character.Instance.transform.position;
        if (Vector3.Distance(this.transform.position, targetPosition) > _maxRange)
        {
            targetPosition = this.transform.position + (targetPosition - this.transform.position).normalized * _maxRange;
        }
        Throw(targetPosition, gernade);

        yield return new WaitForSeconds(_delayBetweenThrow);
        attackable = true;
    }

    void Throw(Vector3 target, GameObject gernade)
    {
        Vector3 direction = target - gernade.transform.position;
        float h = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float a = throwAngleOffset * Mathf.Deg2Rad;
        if (Mathf.Tan(a) == 0)
        {
            gernade.GetComponent<Rigidbody>().AddForce(_maxRange * direction.normalized, ForceMode.Impulse);
        }
        else
        {
            direction.y = distance * Mathf.Tan(a);
            distance += h / Mathf.Tan(a);

            float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
            gernade.GetComponent<Rigidbody>().AddForce(velocity * direction.normalized, ForceMode.Impulse);
        }

    }

    IEnumerator IgnoreCollision(GameObject a)
    {
        Physics.IgnoreCollision(a.GetComponent<Collider>(), transform.parent.gameObject.GetComponent<Collider>(), true);
        yield return new WaitForSeconds(0.1f);
        Physics.IgnoreCollision(a.GetComponent<Collider>(), transform.parent.gameObject.GetComponent<Collider>(), false);
    }

    #endregion
}
