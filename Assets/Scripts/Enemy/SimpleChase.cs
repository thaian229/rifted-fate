using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleChase : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 5f;
    public float meleeDamage = 5f;

    void Awake()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 chaseDirection = (player.position - this.transform.position).normalized;
            this.transform.Translate(chaseDirection * chaseSpeed * Time.deltaTime);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            Damageable damageable = other.gameObject.GetComponent<Damageable>();
            if (damageable)
            {
                damageable.TakeDamage(meleeDamage);
            }
        }
    }
}
