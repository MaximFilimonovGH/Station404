using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityPickup : MonoBehaviour
{

    public bool unlockDoubleJump, unlockDash;
    public int unlockTextDuration = 5;

    public GameObject pickupEffect;
    public string unlockMessage;
    public TMP_Text unlockText;

    private void Start()
    {
        // disable if abilities are already unlocked
        PlayerAbilityTracker playerAbilities = PlayerVitalsController.instance.GetComponent<PlayerAbilityTracker>();
        if (unlockDoubleJump && playerAbilities.canDoubleJump)
        {
            gameObject.SetActive(false);
        }
        if (unlockDash && playerAbilities.canDash)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerAbilityTracker player = other.GetComponentInParent<PlayerAbilityTracker>();

        if(other.tag == "Player")
        {
            if (unlockDoubleJump)
            {
                player.canDoubleJump = true;
                unlockMessage = "Double Jump unlocked";
                PlayerPrefs.SetInt("DoubleJumpUnlocked", 1);
            }
            if (unlockDash)
            {
                player.canDash = true;
                unlockMessage = "Dash unlocked";
                PlayerPrefs.SetInt("DashUnlocked", 1);
            }

            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, transform.rotation);
            }

            unlockText.transform.parent.SetParent(null);
            unlockText.transform.parent.position = transform.position;

            unlockText.text = unlockMessage;
            unlockText.gameObject.SetActive(true);

            Destroy(unlockText.transform.parent.gameObject, unlockTextDuration);

            Destroy(gameObject);
        }
    }

}
