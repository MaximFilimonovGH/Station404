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

    // Start is called before the first frame update
    void Start()
    {
        // update vitals with current values
        UpdateHealth(PlayerVitalsController.instance.currentHealth, PlayerVitalsController.instance.maxHealth);
        UpdateThirst(PlayerVitalsController.instance.currentThirst, PlayerVitalsController.instance.maxThirst);

        fadeScreen.enabled = false;

        // find player
        player = PlayerVitalsController.instance.GetComponent<PlayerController>();
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
    }

    // functions to manipulate vital sliders
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void UpdateThirst(int currentThirst, int maxThirst)
    {
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
