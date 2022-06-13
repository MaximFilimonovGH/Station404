using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleController : MonoBehaviour
{
    public int doorNumber;
    public GameObject activateEffect;

    public TMP_Text activateButtonText;
    public float distanceToShowActivate;
    private bool readyToActivate;

    public GameObject dialogueBox;
    public GameObject pauseScreen;

    public GameObject consoleErrorBox, consoleAccessGrantedBox;

    private PlayerController player;
    private PlayerProgressTracker gameProgress;

    private bool dialogueBoxShowing;

    public GameObject correctChoice;

    public Animator doorAnim;

    // Start is called before the first frame update
    void Start()
    {
        gameProgress = PlayerVitalsController.instance.GetComponent<PlayerProgressTracker>();
        player = PlayerVitalsController.instance.GetComponent<PlayerController>();
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
        if ((Input.GetButtonDown("Submit") && readyToActivate && doorNumber == 0 && !gameProgress.zeroDoorOpen) ||
            (Input.GetButtonDown("Submit") && readyToActivate && doorNumber == 1 && !gameProgress.firstDoorOpen) ||
            (Input.GetButtonDown("Submit") && readyToActivate && doorNumber == 2 && !gameProgress.secondDoorOpen) ||
            (Input.GetButtonDown("Submit") && readyToActivate && doorNumber == 3 && !gameProgress.thirdDoorOpen) ||
            (Input.GetButtonDown("Submit") && readyToActivate && doorNumber == 4 && !gameProgress.fourthDoorOpen))
        {
            ConsoleBoxOpen();
        }
    }

    public void ConsoleBoxOpen()
    {
        // set correct option if conditions met
        if ((gameProgress.zeroJournalPageFound && doorNumber == 0) ||
            (gameProgress.firstJournalPageFound && doorNumber == 1) ||
            (gameProgress.secondJournalPageFound && doorNumber == 2) ||
            (gameProgress.thirdJournalPageFound && doorNumber == 3) ||
            (gameProgress.fourthJournalPageFound && doorNumber == 4))
        {
            correctChoice.SetActive(true);
        } else
        {
            correctChoice.SetActive(false);
        }

        pauseScreen.SetActive(true);
        dialogueBoxShowing = true;
        dialogueBox.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseConsoleAfterAccessGranted()
    {
        // unfreeze time
        Time.timeScale = 1f;
        // close dialogue box
        consoleAccessGrantedBox.SetActive(false);
        dialogueBoxShowing = false;
        pauseScreen.SetActive(false);

        if (doorNumber == 0)
        {
            gameProgress.zeroDoorOpen = true;
        }
        else if (doorNumber == 1)
        {
            gameProgress.firstDoorOpen = true;
        }
        else if (doorNumber == 2)
        {
            gameProgress.secondDoorOpen = true;
        }
        else if (doorNumber == 3)
        {
            gameProgress.thirdDoorOpen = true;
        }
        else if (doorNumber == 4)
        {
            gameProgress.fourthDoorOpen = true;
        }

        doorAnim.SetBool("doorOpen", true);
    }

    public void OpenErrorBox()
    {
        // close dialogue box
        dialogueBox.SetActive(false);
        // open error box
        consoleErrorBox.SetActive(true);
    }

    public void OpenAccessBox()
    {
        // close dialogue box
        dialogueBox.SetActive(false);
        // open error box
        consoleAccessGrantedBox.SetActive(true);
    }

    public void CloseConsoleAfterError()
    {
        // unfreeze time
        Time.timeScale = 1f;
        // close error box
        consoleErrorBox.SetActive(false);
        dialogueBoxShowing = false;
        pauseScreen.SetActive(false);
    }

    public void CloseConsole()
    {
        // unfreeze time
        Time.timeScale = 1f;
        // close error box
        dialogueBox.SetActive(false);
        dialogueBoxShowing = false;
        pauseScreen.SetActive(false);
    }
}
