using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomController : MonoBehaviour
{
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

    public int[] journalPagesNeeded;
    private bool conditionsMet = true;

    public Transform activatePoint;

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
        if (Vector3.Distance(activatePoint.position, player.transform.position) < distanceToShowActivate)
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
        }
    }

    public void ConsoleBoxOpen()
    {
        // check if conditions are met for showing correct option
        conditionsMet = true;
        foreach (int i in journalPagesNeeded)
        {
            if (!gameProgress.journalPagesFound[i])
            {
                conditionsMet = false;
                break;
            }
        }
        if (conditionsMet)
        {
            correctChoice.SetActive(true);
        }
        else
        {
            correctChoice.SetActive(false);
        }

        dialogueBox.SetActive(true);
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

        PlayerVitalsController.instance.fillVital("health", "fridge");
        PlayerVitalsController.instance.fillVital("thirst", "fridge");
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
