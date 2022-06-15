using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    public float HealAmount;

    protected override void OnPicked(PlayerMovement player)
    {
        Damageable playerHealth = player.GetComponent<Damageable>();
        if (playerHealth)
        {
            playerHealth.TakeDamage(-HealAmount);
            PlayPickupFeedback();
            Destroy(gameObject);
        }
    }
}
