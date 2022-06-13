using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVitalsController : MonoBehaviour
{
    public static PlayerVitalsController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float invincibilityLength;
    private float invincibilityCounter;

    public float flashLength;
    private float flashCounter;

    public SpriteRenderer playerSprite;

    public int currentHealth;
    public int maxHealth;
    public int startingHealth;

    public int currentThirst;
    public int maxThirst;
    public int startingThirst;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        currentThirst = startingThirst;
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
        UIController.instance.UpdateThirst(currentThirst, maxThirst);
    }

    // Update is called once per frame
    void Update()
    {
        // if took damage and invincible, start flashing for flashLength
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;

            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                playerSprite.enabled = !playerSprite.enabled;
                flashCounter = flashLength;
            }

            if (invincibilityCounter <= 0)
            {
                playerSprite.enabled = true;
                flashCounter = 0f;
            }
        }
    }

    public void DamagePlayerVital(int damageAmount, string vitalSignal)
    {
        if (vitalSignal == "health")
        {
            if (invincibilityCounter <= 0)
            {
                currentHealth -= damageAmount;

                if (currentHealth <= 0)
                {
                    currentHealth = 0;

                    RespawnController.instance.Respawn();
                }
                else
                {
                    invincibilityCounter = invincibilityLength;
                }
            }
            UIController.instance.UpdateHealth(currentHealth, maxHealth);
        }
        else if (vitalSignal == "thirst")
        {
            currentThirst -= damageAmount;

            if (currentThirst <= 0)
            {
                currentThirst = 0;
                RespawnController.instance.Respawn();
            }
            UIController.instance.UpdateThirst(currentThirst, maxThirst);
        }

    }

    public void fillVital(string vitalSignal)
    {
        if (vitalSignal == "health")
        {
            currentHealth = maxHealth;
            UIController.instance.UpdateHealth(currentHealth, maxHealth);
        }
        else if (vitalSignal == "thirst")
        {
            currentThirst = maxThirst;
            UIController.instance.UpdateThirst(currentThirst, maxThirst);
        }
    }


    public void healPlayerVital(int healAmount, string vitalSignal)
    {
        if (vitalSignal == "health")
        {
            currentHealth += healAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            UIController.instance.UpdateHealth(currentHealth, maxHealth);
        }
        else if (vitalSignal == "thirst")
        {
            currentThirst += healAmount;
            if (currentThirst > maxThirst)
            {
                currentThirst = maxThirst;
            }
            UIController.instance.UpdateThirst(currentThirst, maxThirst);
        }
        
    }
}
