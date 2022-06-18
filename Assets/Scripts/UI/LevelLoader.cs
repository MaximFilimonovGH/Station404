using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    // start level text
    public GameObject pauseScreen;
    public GameObject startLevelTextBox;
    public GameObject menuBackgroundImage;
    private bool startLevelTextShowing = false;

    public string[] messages;
    private int messagesCounter;

    public TMP_Text dialogueBoxText;
    public TMP_Text buttonText;

    private PlayerProgressTracker gameProgress;
    private PlayerController player;

    public string finalButtonText;
    private int levelNumber;

    private void OnEnable()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string levelNumberString = sceneName.Substring(sceneName.LastIndexOf(" ") + 1);
        levelNumber = int.Parse(levelNumberString);

        // find player
        // instantiate him if null
        if (PlayerVitalsController.instance == null)
        {
            PlayerVitalsController newPVC = Instantiate(FindObjectOfType<PlayerVitalsController>());
            PlayerVitalsController.instance = newPVC;
            DontDestroyOnLoad(newPVC.gameObject);
        }
        if (PlayerVitalsController.instance != null)
        {
            player = PlayerVitalsController.instance.GetComponent<PlayerController>();
            gameProgress = PlayerVitalsController.instance.GetComponent<PlayerProgressTracker>();
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        // find player
        //player = PlayerVitalsController.instance.GetComponent<PlayerController>();
        //gameProgress = PlayerVitalsController.instance.GetComponent<PlayerProgressTracker>();
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // show only when level is not already visited
        if (!gameProgress.levelsVisited[levelNumber])
        {
            if (levelNumber == 0)
            {
                ShowStartLevelText();
            } else
            {
                StartCoroutine(ShowStartLevelTextCo());
            }
        }
    }

    void ShowStartLevelText()
    {
        if (menuBackgroundImage != null)
        {
            menuBackgroundImage.SetActive(true);
        }

        dialogueBoxText.text = messages[0];
        buttonText.text = "Next";
        if (messages.Length == 1)
        {
            buttonText.text = finalButtonText;
        }

        startLevelTextShowing = true;
        startLevelTextBox.SetActive(true);

        Time.timeScale = 0f;
        AudioManager.instance.PlaySFXAdjusted(5);
    }

    IEnumerator ShowStartLevelTextCo()
    {
        yield return new WaitForSeconds(0.5f);

        pauseScreen.SetActive(true);

        dialogueBoxText.text = messages[0];
        buttonText.text = "Next";
        if (messages.Length == 1)
        {
            buttonText.text = finalButtonText;
        }

        startLevelTextShowing = true;
        startLevelTextBox.SetActive(true);

        Time.timeScale = 0f;
        AudioManager.instance.PlaySFXAdjusted(5);
    }

    // Update is called once per frame
    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string levelNumberString = sceneName.Substring(sceneName.LastIndexOf(" ") + 1);
        levelNumber = int.Parse(levelNumberString);

        // if startLevelTextShowing player cannot move
        if (startLevelTextShowing)
        {
            player.canMove = false;
        }
        else
        {
            player.canMove = true;
        }
    }

    // functions for operation with start level text
    public void NextMessage()
    {
        if (messagesCounter == messages.Length - 1)
        {
            StartGameLevel();
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

    public void StartGameLevel()
    {
        // close dialogue box
        startLevelTextBox.SetActive(false);
        startLevelTextShowing = false;
        AudioManager.instance.PlaySFXAdjusted(5);
        // if level 0, disable red menu space
        if (levelNumber == 0)
        {
            // if there is a background image covering the game as well (level 0) then deactivate it
            if (menuBackgroundImage != null)
            {
                menuBackgroundImage.SetActive(false);
            }
        } else
        {
            pauseScreen.SetActive(false);
        }

        gameProgress.levelsVisited[levelNumber] = true;
        PlayerPrefs.SetInt(string.Concat("LevelVisited_", levelNumber.ToString()), 1);

        // unfreeze time
        Time.timeScale = 1f;
    }
}
