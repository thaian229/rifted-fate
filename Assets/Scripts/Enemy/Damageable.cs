using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float health = 20f;

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
            
            // Broadcast event
            EnemyKillEvent evt = Events.EnemyKillEvent;
            evt.Enemy = this.gameObject;
            EventManager.Broadcast(evt);

            Destroy(this.gameObject);
        }
        if (this.gameObject.tag == "Player")
        {
            LevelManager.instance.GameOver();
        }
    }
}
