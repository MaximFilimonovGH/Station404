using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FirstGooController : MonoBehaviour
{
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
        player = PlayerVitalsController.instance.GetComponent<PlayerController>();
        gameProgress = PlayerVitalsController.instance.GetComponent<PlayerProgressTracker>();
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
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToShowActivate && !gameProgress.gameFinished)
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
    }
}
