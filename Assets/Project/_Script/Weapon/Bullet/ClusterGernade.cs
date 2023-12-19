using System.Collections;
using System.Collections.Generic;
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

        List<Collider> colliders = new List<Collider>();
        List<GameObject> gernades = new List<GameObject>();
        for (int a = 0; a < smallerGernadeCount; a++)
        {
            GameObject gernade = Instantiate(smallerGernade, this.transform.position, new Quaternion());
            gernade.tag = this.tag;
            colliders.Add(gernade.GetComponent<Collider>());
            gernades.Add(gernade);
        }
        for (int i = 0; i < colliders.Count; i++)
        {
            Physics.IgnoreCollision(colliders[i], this.GetComponent<Collider>());
            for (int j = 0; j < i; j++)
            {
                Physics.IgnoreCollision(colliders[i], colliders[j]);
            }
            Vector3 direction = Random.insideUnitSphere.normalized;
            gernades[i].GetComponent<Rigidbody>().AddForce(direction * smallerGernadeThrowForce, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }
}

