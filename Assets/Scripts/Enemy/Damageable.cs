using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float maxHP = 20f;
    public float health = 20f;

    void Start()
    {
        health = maxHP;
    }

    public void InitMaxHealth(float hp) 
    {
        maxHP = hp;
        health = hp;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0.1f)
        {
            Die();
        }
    }

    private void Die()
    {
        
        if (this.gameObject.tag == "Enemy")
        {
            // Gain Reward
            LevelManager.instance.EarnScore(1);

            // Drop loot
            GameObject lootPrefab = gameObject.GetComponent<EnemyController>().LootPrefab;
            float dropRate = gameObject.GetComponent<EnemyController>().DropRate;
            float chance = Random.Range(0, 1);
            if (chance <= dropRate) {
                Instantiate(lootPrefab, transform.position, Quaternion.identity);
            }
            
            // Broadcast event
            EnemyKillEvent evt = Events.EnemyKillEvent;
            evt.Enemy = this.gameObject;
            EventManager.Broadcast(evt);

            Destroy(this.gameObject);
        }
        if (this.gameObject.tag == "Player")
        {
            LevelManager.instance.GameOver(false);
        }
    }
}
