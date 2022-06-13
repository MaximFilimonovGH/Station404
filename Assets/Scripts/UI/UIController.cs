using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Slider healthSlider, thirstSlider;

    public Image fadeScreen;

    public float fadeSpeed = 2;
    private bool fadingToBlack, fadingFromBlack;

    public string mainMenuScene;

    public GameObject pauseScreen;
    private PlayerController player;

    // start level text
    public bool needToShowStartLevelText = false;
    public GameObject startLevelTextBox;
    public GameObject menuBackgroundImage;
    private bool startLevelTextShowing = false;

    // Start is called before the first frame update
    void Start()
    {
        // update vitals with current values
        //UpdateHealth(PlayerVitalsController.instance.currentHealth, PlayerVitalsController.instance.maxHealth);
        //UpdateThirst(PlayerVitalsController.instance.currentThirst, PlayerVitalsController.instance.maxThirst);

        // disable fadescreen so that can select button through
        fadeScreen.enabled = false;

        // find player
        player = PlayerVitalsController.instance.GetComponent<PlayerController>();

        // if first level show dialogue box and background from the start
        if (needToShowStartLevelText)
        {
            startLevelTextShowing = true;
            startLevelTextBox.SetActive(true);
            if (menuBackgroundImage != null)
            {
                menuBackgroundImage.SetActive(true);
            }
            Time.timeScale = 0f;
        }
        else
        {
            startLevelTextShowing = false;
            startLevelTextBox.SetActive(false);
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
        if (fadingToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 1f)
            {
                fadingToBlack = false;
            }
        }
        else if (fadingFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 0f)
            {
                fadingFromBlack = false;
                fadeScreen.enabled = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }

        // if startLevelTextShowing player cannot move
        if (startLevelTextShowing)
        {
            player.canMove = false;
        }
        else
        {
            player.canMove = true;
        }
        // if we pressed submit and start level text is showing
        if (Input.GetButtonDown("Submit") && startLevelTextShowing)
        {
            StartTextBoxClose();
        }
    }
    
    // functions for operation with start level text
    public void StartTextBoxClose()
    {
        StartCoroutine(StartGameLevelCo());
    }

    IEnumerator StartGameLevelCo()
    {
        // unfreeze time
        Time.timeScale = 1f;

        startFadeToBlack();

        yield return new WaitForSeconds(1.5f);

        // close dialogue box
        startLevelTextBox.SetActive(false);
        startLevelTextShowing = false;
        // if there is a background image covering the game as well (level 0) then deactivate it
        if (menuBackgroundImage != null)
        {
            menuBackgroundImage.SetActive(false);
        }

        startFadeFromBlack();
    }

    // functions to manipulate vital sliders
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        Debug.Log("called");
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void UpdateThirst(int currentThirst, int maxThirst)
    {
        Debug.Log("called");
        thirstSlider.maxValue = maxThirst;
        thirstSlider.value = currentThirst;
    }

    // fade/unfade
    public void startFadeToBlack()
    {
        fadeScreen.enabled = true;
        fadingToBlack = true;
        fadingFromBlack = false;
    }

    public void startFadeFromBlack()
    {
        fadingToBlack = false;
        fadingFromBlack = true;
    }

    public void PauseUnpause()
    {
        if(!pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);

            Time.timeScale = 1f;
        }
    }

    public void GoToMainMenu()
    {
        Destroy(PlayerVitalsController.instance.gameObject);
        PlayerVitalsController.instance = null;

        Destroy(RespawnController.instance.gameObject);
        RespawnController.instance = null;

        instance = null;
        Destroy(gameObject);

        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
