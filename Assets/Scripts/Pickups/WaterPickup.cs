using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPickup : MonoBehaviour
{
    public int healAmount;

    public GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // do nothing if health is already full
            if (PlayerVitalsController.instance.currentThirst == PlayerVitalsController.instance.maxThirst)
            {

            }
            else
            {
                PlayerVitalsController.instance.healPlayerVital(healAmount, "thirst");

                if (pickupEffect != null)
                {
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);
            }

        }
    }
}
