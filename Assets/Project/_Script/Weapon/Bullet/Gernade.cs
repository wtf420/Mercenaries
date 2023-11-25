using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Gernade : MonoBehaviour
{
    [SerializeField] protected float explosionTimer, _explosionRadius, _damage;
    [SerializeField] protected bool _damageScaleWithDistance;

    public virtual void Update()
    {
        if (explosionTimer > 0)
        {
            explosionTimer -= Time.deltaTime;
        } else
        {
            Explode();
        }
    }

    protected virtual void Explode()
    {
        RaycastHit[] info = Physics.SphereCastAll(this.transform.position, _explosionRadius, Vector3.up, _explosionRadius);
        foreach (RaycastHit hit in info)
        {
            if (this.tag == hit.transform.tag)
            {
                continue;
            }
            bool c = false;
            RaycastHit[] info2 = Physics.RaycastAll(this.transform.position, hit.transform.position, Vector3.Distance(this.transform.position, hit.transform.position));
            foreach (RaycastHit hit2 in info2)
            {
                //theres an object blocking
                if (hit2.collider.gameObject.GetComponent<BulletproofWall>() || (hit2.collider.gameObject.GetComponent<IDamagable>() == null))
                {
                    if (hit2.collider.gameObject.GetComponent<BulletproofWall>())
                    {
                        if (_damageScaleWithDistance)
                        {
                            float distance = Vector3.Distance(this.transform.position, hit.point);
                            hit2.collider.gameObject.GetComponent<BulletproofWall>().TakenDamage(_damage * (distance / _explosionRadius));
                        }
                        else
                            hit2.collider.gameObject.GetComponent<BulletproofWall>().TakenDamage(_damage);
                        return;
                    }
                    c = true;
                }
            }

            if (c) continue;

            if (hit.collider.gameObject.GetComponent<IDamagable>() != null)
            {
                float distance = Vector3.Distance(this.transform.position, hit.point);
                float Damage = _damageScaleWithDistance ? _damage * (1 / (distance / _explosionRadius)) : _damage;
                hit.collider.gameObject.GetComponent<IDamagable>().TakenDamage(Damage);
                if (hit.collider.gameObject.GetComponent<Enemy>())
                {
                    hit.collider.gameObject.GetComponent<Enemy>().TakenDamage(Damage, hit.point - hit.transform.position, 10f);
                }
                else
                {
                    hit.collider.gameObject.GetComponent<IDamagable>().TakenDamage(Damage);
                }
            }
        }
        StopAllCoroutines();
        Destroy(gameObject);
    }

    [ExecuteInEditMode]
    protected virtual void OnDrawGizmos()
    {
        if (this.tag == "Player")
            Gizmos.color = Color.blue;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(this.transform.position, _explosionRadius);
    }
}

