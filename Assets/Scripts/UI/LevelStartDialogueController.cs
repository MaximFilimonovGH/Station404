using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelStartDialogueController : MonoBehaviour
{
    public GameObject dialogueBox;
    public GameObject menuBackgroundImage;

    private bool dialogueBoxShowing;
    public bool needToShow = false;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerVitalsController.instance.GetComponent<PlayerController>();
        // if first level show dialogue box and background from the start
        if (needToShow)
        {
            dialogueBoxShowing = true;
            dialogueBox.SetActive(true);
            if (menuBackgroundImage != null)
            {
                menuBackgroundImage.SetActive(true);
            }
            Time.timeScale = 0f;
        }
        else
        {
            dialogueBoxShowing = false;
            dialogueBox.SetActive(false);
            if (menuBackgroundImage != null)
            {
                menuBackgroundImage.SetActive(false);
            }
            Time.timeScale = 1f;
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
        if (Input.GetButtonDown("Submit") && dialogueBoxShowing)
        {
            DialogueBoxClose();
        }
    }

    public void DialogueBoxClose()
    {
        StartCoroutine(StartGameLevelCo());
    }

    IEnumerator StartGameLevelCo()
    {
        // unfreeze time
        Time.timeScale = 1f;

        UIController.instance.startFadeToBlack();

        yield return new WaitForSeconds(1.5f);

        // close dialogue box
        dialogueBox.SetActive(false);
        dialogueBoxShowing = false;
        // if there is a background image covering the game as well (level 0) then deactivate it
        if (menuBackgroundImage != null)
        {
            menuBackgroundImage.SetActive(false);
        }

        UIController.instance.startFadeFromBlack();
    }
}
