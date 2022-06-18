using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleController : MonoBehaviour
{
    public int doorNumber;
    public int[] windowsShattered;
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

    public int[] journalPagesNeeded;
    public bool isFingerNeeded;
    private bool conditionsMet = true;

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

        // detect activate if closeby
        if (Input.GetButtonDown("Submit") && readyToActivate)
        {
            ConsoleBoxOpen();

            // save window shatter progress
            foreach (int i in windowsShattered)
            {
                gameProgress.windowsShattered[i] = true;
                PlayerPrefs.SetInt(string.Concat("WindowShattered_", i.ToString()), 1);
            }
 
        }
    }

    public void ConsoleBoxOpen()
    {
        // check if conditions are met for showing correct option
        conditionsMet = true;
        foreach(int i in journalPagesNeeded)
        {
            if (!gameProgress.journalPagesFound[i])
            {
                conditionsMet = false;
                break;
            }
        }
        if (isFingerNeeded && !gameProgress.fingerFound)
        {
            conditionsMet = false;
        }
        if (conditionsMet)
        {
            correctChoice.SetActive(true);
        } else
        {
            correctChoice.SetActive(false);
        }

        // check if door already open. if so, show access granted dialogue
        if (gameProgress.doorsOpen[doorNumber])
        {
            consoleAccessGrantedBox.SetActive(true);
        }
        else
        {
            dialogueBox.SetActive(true);
        }

        pauseScreen.SetActive(true);
        dialogueBoxShowing = true;

        Time.timeScale = 0f;
        AudioManager.instance.PlaySFXAdjusted(5);
    }

    public void CloseConsoleAfterAccessGranted()
    {
        // unfreeze time
        Time.timeScale = 1f;
        // close dialogue box
        consoleAccessGrantedBox.SetActive(false);
        dialogueBoxShowing = false;
        pauseScreen.SetActive(false);

        // set correct door game progress in the tracker
        gameProgress.doorsOpen[doorNumber] = true;
        PlayerPrefs.SetInt(string.Concat("DoorOpen_", doorNumber.ToString()), 1);

        doorAnim.SetBool("doorOpen", true);
        AudioManager.instance.PlaySFXAdjusted(5);
    }

    public void OpenErrorBox()
    {
        // close dialogue box
        dialogueBox.SetActive(false);
        // open error box
        consoleErrorBox.SetActive(true);
        AudioManager.instance.PlaySFXAdjusted(5);
    }

    public void OpenAccessBox()
    {
        // close dialogue box
        dialogueBox.SetActive(false);
        // open access box
        consoleAccessGrantedBox.SetActive(true);
        AudioManager.instance.PlaySFXAdjusted(5);
    }

    public void CloseConsoleAfterError()
    {
        // unfreeze time
        Time.timeScale = 1f;
        // close error box
        consoleErrorBox.SetActive(false);
        dialogueBoxShowing = false;
        pauseScreen.SetActive(false);
        AudioManager.instance.PlaySFXAdjusted(5);
    }

    public void CloseConsole()
    {
        // unfreeze time
        Time.timeScale = 1f;
        // close error box
        dialogueBox.SetActive(false);
        dialogueBoxShowing = false;
        pauseScreen.SetActive(false);
        AudioManager.instance.PlaySFXAdjusted(5);
    }
}
