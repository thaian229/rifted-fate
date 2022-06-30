using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public float AoeDistance = 3f;

    public void DealAreaDamage(float damage, Vector3 center, GameObject owner)
    {
        Collider[] affectedColliders = Physics.OverlapSphere(center, AoeDistance);
        foreach (var coll in affectedColliders)
        {
            Damageable damageable = coll.GetComponent<Damageable>();
            if (!damageable) continue;
            
            if (owner.tag != coll.gameObject.tag)
            {
                float distance = Vector3.Distance(coll.transform.position, this.transform.position);
                damageable.TakeDamage(damage * (1 - (distance / AoeDistance)));
            }
        }
    }
}
