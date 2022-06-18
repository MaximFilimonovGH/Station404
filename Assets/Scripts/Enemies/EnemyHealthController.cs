using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int totalHealth;
    private int currentHealth;
    public GameObject deathEffect;

    public SpriteRenderer gooSprite;
    public float flashLength, flashDuration;
    private float flashCounter, flashDurationCounter;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = totalHealth;
    }

    private void Update()
    {
        if (flashDurationCounter > 0)
        {
            flashDurationCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                gooSprite.enabled = !gooSprite.enabled;
                flashCounter = flashLength;
            }

            if (flashDurationCounter <= 0)
            {
                gooSprite.enabled = true;
                flashCounter = 0f;
            }
        }
    }

    public void DamageEnemy(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        } else
        {
            flashDurationCounter = flashDuration;
        }
        AudioManager.instance.PlaySFXAdjusted(7);
    }
}
