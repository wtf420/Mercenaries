using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ClusterGernade : Gernade
{
    [SerializeField] GameObject smallerGernade;
    [SerializeField] int smallerGernadeCount;
    [SerializeField] float smallerGernadeThrowForce;

    protected override void Explode()
    {
        RaycastHit[] info = Physics.SphereCastAll(this.transform.position, _explosionRadius, Vector3.up, _explosionRadius);
        foreach (RaycastHit hit in info)
        {
            if (hit.collider.gameObject.GetComponent<Enemy>())
            {
                if (_damageScaleWithDistance)
                {
                    float distance = Vector3.Distance(this.transform.position, hit.point);
                    hit.collider.gameObject.GetComponent<Enemy>().TakenDamage(_damage * (distance / _explosionRadius));
                } else
                    hit.collider.gameObject.GetComponent<Enemy>().TakenDamage(_damage);
            }
        }

        for (int i = 0; i < smallerGernadeCount; i++)
        {
            Vector3 direction = Random.insideUnitSphere.normalized;

            GameObject gernade = Instantiate(smallerGernade, this.transform.position, new Quaternion());
            gernade.GetComponent<Rigidbody>().AddForce(direction.normalized * smallerGernadeThrowForce, ForceMode.Impulse);
            gernade.tag = this.tag;

            Destroy(gameObject);
        }
    }
}

