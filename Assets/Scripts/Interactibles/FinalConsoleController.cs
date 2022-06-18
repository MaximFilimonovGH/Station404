using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalConsoleController : MonoBehaviour
{
    public TMP_Text activateButtonText;
    public float distanceToShowActivate;
    private bool readyToActivate;

    public GameObject dialogueBox;
    public GameObject pauseScreen;

    private PlayerController player;
    private PlayerProgressTracker gameProgress;

    private bool dialogueBoxShowing = false;

    public string endSceneToLoad;

    // Start is called before the first frame update
    void Start()
    {
        gameProgress = PlayerVitalsController.instance.GetComponent<PlayerProgressTracker>();
        player = PlayerVitalsController.instance.GetComponent<PlayerController>();

        if (!gameProgress.gameFinished)
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
        if (Vector3.Distance(transform.position, player.transform.position) < distanceToShowActivate && gameProgress.gameFinished)
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
        dialogueBox.SetActive(true);
        pauseScreen.SetActive(true);
        dialogueBoxShowing = true;
        Time.timeScale = 0f;
    }

    public void CloseConsole()
    {
        // unfreeze time
        Time.timeScale = 1f;
        // close error box
        dialogueBox.SetActive(false);
        dialogueBoxShowing = false;
        pauseScreen.SetActive(false);

        gameProgress.gameFinished = true;

        SceneManager.LoadScene(endSceneToLoad);
    }
}
