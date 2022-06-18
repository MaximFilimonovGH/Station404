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
    private PlayerAbilityTracker playerAbilities;

    private bool dialogueBoxShowing;

    public bool unlockDoubleJump, unlockDash;

    public string[] messages;
    private bool allMessagesCycled;
    private int messagesCounter;
    public string finalButtonText;

    public TMP_Text dialogueBoxText;
    public TMP_Text buttonText;


    // Start is called before the first frame update
    void Start()
    {
        gameProgress = PlayerVitalsController.instance.GetComponent<PlayerProgressTracker>();
        playerAbilities = PlayerVitalsController.instance.GetComponent<PlayerAbilityTracker>();
        player = PlayerVitalsController.instance.GetComponent<PlayerController>();

        // check if already found page
        //if (gameProgress.journalPagesFound[journalPageNumber])
        //{
        //    gameObject.SetActive(false);
        //}
        //else
        //{
        //    gameObject.SetActive(true);
        //}
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
        
        dialogueBoxText.text = messages[0];
        buttonText.text = "Next";
        if (messages.Length == 1)
        {
            buttonText.text = finalButtonText;
        }

        pauseScreen.SetActive(true);
        dialogueBoxShowing = true;
        dialogueBox.SetActive(true);
        Time.timeScale = 0f;
        AudioManager.instance.PlaySFXAdjusted(5);
    }

    public void DialogueBoxClose()
    {
        if (messagesCounter == messages.Length - 1)
        {
            // unfreeze time
            Time.timeScale = 1f;
            // close dialogue box
            dialogueBox.SetActive(false);
            dialogueBoxShowing = false;
            pauseScreen.SetActive(false);

            messagesCounter = 0;


            // new way of setting correct pages progress
            gameProgress.journalPagesFound[journalPageNumber] = true;
            PlayerPrefs.SetInt(string.Concat("JournalPageFound_", journalPageNumber.ToString()), 1);

            // unlock necessary abilities
            if (unlockDoubleJump)
            {
                playerAbilities.canDoubleJump = true;
                PlayerPrefs.SetInt("DoubleJumpUnlocked", 1);
            }
            if (unlockDash)
            {
                playerAbilities.canDash = true;
                PlayerPrefs.SetInt("DashUnlocked", 1);
            }
            AudioManager.instance.PlaySFXAdjusted(5);
        }
        else
        {
            messagesCounter++;
            dialogueBoxText.text = messages[messagesCounter];
            if (messagesCounter == messages.Length - 1)
            {
                buttonText.text = finalButtonText;
            }
        }
        
    }
}
