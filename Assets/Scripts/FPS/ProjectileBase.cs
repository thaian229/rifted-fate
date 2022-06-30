using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public LayerMask HittableLayers = -1;
    public float MaxLifetime = 5f;
    public GameObject ImpactVfx;
    public float ImpactVfxLifetime = 1f;
    public AudioClip ImpactSfx;
    public float Speed = 50f;
    public float GravityAcceleration = 0f;
    public float Damage = 10f;
    public DamageArea AreaOfDamage;

    Vector3 m_Velocity;

    void OnEnable()
    {
        m_Velocity = transform.forward * Speed;

        Destroy(this.gameObject, MaxLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        // Move
        transform.position += m_Velocity * Time.deltaTime;

        // Orient towards velocity
        transform.forward = m_Velocity.normalized;

        // Gravity
        if (GravityAcceleration > 0f)
        {
            // add gravity to the projectile velocity for ballistic effect
            m_Velocity += Vector3.down * GravityAcceleration * Time.deltaTime;
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (AreaOfDamage)
            {
                // Deal damage in an area
                AreaOfDamage.DealAreaDamage(Damage, this.transform.position, this.gameObject);
            }
            else
            {
                // Deal damage
                Damageable damageable = other.gameObject.GetComponent<Damageable>();
                if (damageable)
                {
                    damageable.TakeDamage(Damage);
                }
            }
        }
        // Self destroy
        Destroy(this.gameObject);
    }
}
