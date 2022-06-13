using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalPickup : MonoBehaviour
{
 
    public int journalPageNumber;
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

        // check if already found page
        if ((journalPageNumber == 0 && gameProgress.zeroJournalPageFound) ||
            (journalPageNumber == 1 && gameProgress.firstJournalPageFound) ||
            (journalPageNumber == 2 && gameProgress.secondJournalPageFound) ||
            (journalPageNumber == 3 && gameProgress.thirdJournalPageFound) ||
            (journalPageNumber == 4 && gameProgress.fourthJournalPageFound))
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
    }

    public void DialogueBoxClose()
    {
        // unfreeze time
        Time.timeScale = 1f;
        // close dialogue box
        dialogueBox.SetActive(false);
        dialogueBoxShowing = false;
        pauseScreen.SetActive(false);

        // set correct page progress
        if (journalPageNumber == 0)
        {
            gameProgress.zeroJournalPageFound = true;
        } 
        else if(journalPageNumber == 1)
        {
            gameProgress.firstJournalPageFound = true;
        }
        else if (journalPageNumber == 2)
        {
            gameProgress.secondJournalPageFound = true;
        }
        else if (journalPageNumber == 3)
        {
            gameProgress.thirdJournalPageFound = true;
        }
        else if (journalPageNumber == 4)
        {
            gameProgress.fourthJournalPageFound = true;
        }
    }
}
