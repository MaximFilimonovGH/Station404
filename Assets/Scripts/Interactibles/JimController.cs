using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class JimController : MonoBehaviour
{

    public TMP_Text activateButtonText;
    public float distanceToShowActivate;
    private bool readyToActivate;
    public Transform activationPoint;

    public GameObject dialogueBox;
    public GameObject pauseScreen;

    private PlayerController player;
    private PlayerProgressTracker gameProgress;

    private bool dialogueBoxShowing;

    public string[] messages;
    private bool allMessagesCycled;
    private int messagesCounter;
    public string finalButtonText;

    public TMP_Text dialogueBoxText;
    public TMP_Text buttonText;

    public GameObject choiceBox, stayChoiceBox;

    private bool playerExiting;
    public Transform exitPoint;
    public string runLevelToLoad;
    public string endLevelToLoad;

    public GameObject correctChoice;
    private bool conditionsMet = false;

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
        if (Vector3.Distance(activationPoint.position, player.transform.position) < distanceToShowActivate)
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

        if (playerExiting)
        {
            player.transform.position = exitPoint.position;
        }
    }

    public void DialogueBoxOpen()
    {
        // check if conditions are met
        if (gameProgress.journalPagesFound[6])
        {
            correctChoice.SetActive(true);
        }
        else
        {
            correctChoice.SetActive(false);
        }

        dialogueBoxText.text = messages[0];
        buttonText.text = "Next";
        if (messages.Length == 1)
        {
            buttonText.text = finalButtonText;
        }

        if (gameProgress.timeToChoose)
        {
            choiceBox.SetActive(true);
        } else
        {
            dialogueBox.SetActive(true);
        }

        pauseScreen.SetActive(true);
        dialogueBoxShowing = true;

        Time.timeScale = 0f;
        AudioManager.instance.PlaySFXAdjusted(5);
    }

    public void NextMessage()
    {

        if (messagesCounter == messages.Length - 1)
        {
            OpenChoiceBox();
            messagesCounter = 0;
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
        AudioManager.instance.PlaySFXAdjusted(5);
    }

    public void OpenChoiceBox()
    {
        // close dialogue box
        dialogueBox.SetActive(false);
        // open access box
        choiceBox.SetActive(true);
        AudioManager.instance.PlaySFXAdjusted(5);
        gameProgress.timeToChoose = true;
    }

    public void OpenStayChoiceBox()
    {
        // close dialogue box
        choiceBox.SetActive(false);
        // open access box
        stayChoiceBox.SetActive(true);
        AudioManager.instance.PlaySFXAdjusted(5);
    }

    public void FinishGame()
    {
        // unfreeze time
        Time.timeScale = 1f;
        // close dialogue box
        stayChoiceBox.SetActive(false);
        dialogueBoxShowing = false;
        pauseScreen.SetActive(false);
        AudioManager.instance.PlaySFXAdjusted(5);

        gameProgress.gameFinished = true;
        PlayerPrefs.SetInt("GameFinished", 1);
        
        SceneManager.LoadScene(endLevelToLoad);
    }

    public void RunSelected()
    {
        // unfreeze time
        Time.timeScale = 1f;
        // close  box
        choiceBox.SetActive(false);
        dialogueBoxShowing = false;
        pauseScreen.SetActive(false);
        gameProgress.gameFinished = true;
        gameProgress.doorsOpen[3] = true;
        gameProgress.doorsOpen[0] = false;
        gameProgress.doorsOpen[1] = false;
        gameProgress.doorsOpen[2] = false;
        PlayerPrefs.SetInt("GameFinished", 1);
        PlayerPrefs.SetInt("DoorOpen_3", 1);
        PlayerPrefs.SetInt("DoorOpen_0", 0);
        PlayerPrefs.SetInt("DoorOpen_1", 0);
        PlayerPrefs.SetInt("DoorOpen_2", 0);
        AudioManager.instance.PlaySFXAdjusted(5);

        player.canMove = false;

        StartCoroutine(RunCo());
    }

    IEnumerator RunCo()
    {
        playerExiting = true;
        player.isExitingLevel = true;

        player.thePlayerAnimations.enabled = false;

        UIController.instance.startFadeToBlack();

        yield return new WaitForSeconds(1.5f);

        RespawnController.instance.SetSpawn(exitPoint.position);
        player.canMove = true;
        player.thePlayerAnimations.enabled = true;

        UIController.instance.startFadeFromBlack();
        // UIController.instance.messages = messages;

        PlayerPrefs.SetString("ContinueLevel", runLevelToLoad);
        PlayerPrefs.SetFloat("PosX", exitPoint.position.x);
        PlayerPrefs.SetFloat("PosY", exitPoint.position.y);
        PlayerPrefs.SetFloat("PosZ", exitPoint.position.z);

        player.isExitingLevel = false;

        SceneManager.LoadScene(runLevelToLoad);
    }

    public void CloseChoiceBox()
    {
        // unfreeze time
        Time.timeScale = 1f;
        // close  box
        choiceBox.SetActive(false);
        dialogueBoxShowing = false;
        pauseScreen.SetActive(false);
    }

}
