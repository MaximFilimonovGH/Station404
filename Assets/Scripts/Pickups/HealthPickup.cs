using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public int healAmount;

    public GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // do nothing if health is already full
            if (PlayerVitalsController.instance.currentHealth == PlayerVitalsController.instance.maxHealth)
            {

            } else
            {
                PlayerVitalsController.instance.healPlayerVital(healAmount, "health");
                AudioManager.instance.PlaySFX(3);

                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }

        }
    }
}
