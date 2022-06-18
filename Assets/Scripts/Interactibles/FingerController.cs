using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FingerController : MonoBehaviour
{
    public GameObject pickupEffect;

    public TMP_Text activateButtonText;
    public float distanceToShowActivate;
    private bool readyToActivate;

    public GameObject dialogueBox;
    public GameObject pauseScreen;

    private PlayerController player;
    private PlayerProgressTracker gameProgress;

    private bool dialogueBoxShowing;

    // Start is called before the first frame update
    void Start()
    {
        gameProgress = PlayerVitalsController.instance.GetComponent<PlayerProgressTracker>();
        player = PlayerVitalsController.instance.GetComponent<PlayerController>();

        // check if already found finger
        if (gameProgress.fingerFound)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueBoxShowing)
        {
            player.canMove = false;
        }
        else
        {
            player.canMove = true;
        }

        // show activate text only when closeby
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToShowActivate)
        {
            readyToActivate = true;
            activateButtonText.gameObject.SetActive(true);
        }
        else
        {
            readyToActivate = false;
            activateButtonText.gameObject.SetActive(false);
        }

        // detect activate
        if (Input.GetButtonDown("Submit") && readyToActivate)
        {
            DialogueBoxOpen();
        }
    }

    public void DialogueBoxOpen()
    {
        pauseScreen.SetActive(true);
        dialogueBoxShowing = true;
        dialogueBox.SetActive(true);
        Time.timeScale = 0f;
        AudioManager.instance.PlaySFXAdjusted(5);
    }

    public void DialogueBoxClose()
    {
        // unfreeze time
        Time.timeScale = 1f;
        // close dialogue box
        dialogueBox.SetActive(false);
        dialogueBoxShowing = false;
        pauseScreen.SetActive(false);
        AudioManager.instance.PlaySFXAdjusted(5);

        // set progress for finger
        gameProgress.fingerFound = true;
        PlayerPrefs.SetInt("FingerFound", 1);

        // play effect and destroy object
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
