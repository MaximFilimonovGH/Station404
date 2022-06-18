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
        if (PlayerPrefs.HasKey("Health"))
        {
            currentHealth = PlayerPrefs.GetInt("Health");
        } else
        {
            currentHealth = startingHealth;
        }
        if (PlayerPrefs.HasKey("Thirst"))
        {
            currentThirst = PlayerPrefs.GetInt("Thirst");
        }
        else
        {
            currentThirst = startingThirst;
        }
        if (UIController.instance != null)
        {
            UIController.instance.UpdateHealth(currentHealth, maxHealth);
            UIController.instance.UpdateThirst(currentThirst, maxThirst);
        }
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
                AudioManager.instance.PlaySFXAdjusted(6);

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
            PlayerPrefs.SetInt("Health", currentHealth);
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
            PlayerPrefs.SetInt("Thirst", currentThirst);
        }
    }

    public void fillVital(string vitalSignal, string keyWord)
    {
        if (vitalSignal == "health" && keyWord == "respawn")
        {
            currentHealth = maxHealth / 2;
            UIController.instance.UpdateHealth(currentHealth, maxHealth);
            PlayerPrefs.SetInt("Health", currentHealth);
        }
        else if (vitalSignal == "thirst" && keyWord == "respawn")
        {
            currentThirst = maxThirst / 2;
            UIController.instance.UpdateThirst(currentThirst, maxThirst);
            PlayerPrefs.SetInt("Thirst", currentThirst);
        }
        else if (vitalSignal == "health" && keyWord == "fridge")
        {
            currentHealth = maxHealth;
            UIController.instance.UpdateHealth(currentHealth, maxHealth);
            PlayerPrefs.SetInt("Health", currentHealth);
        }
        else if (vitalSignal == "thirst" && keyWord == "fridge")
        {
            currentThirst = maxThirst;
            UIController.instance.UpdateThirst(currentThirst, maxThirst);
            PlayerPrefs.SetInt("Thirst", currentThirst);
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
            PlayerPrefs.SetInt("Health", currentHealth);
        }
        else if (vitalSignal == "thirst")
        {
            currentThirst += healAmount;
            if (currentThirst > maxThirst)
            {
                currentThirst = maxThirst;
            }
            UIController.instance.UpdateThirst(currentThirst, maxThirst);
            PlayerPrefs.SetInt("Thirst", currentThirst);
        }
        
    }
}
