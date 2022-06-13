using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string newGameScene;

    public GameObject continueButton;

    public PlayerAbilityTracker playerAbilities;

    // Start is called before the first frame update
    void Start()
    {
        // check if there is any saved progress
        if(PlayerPrefs.HasKey("ContinueLevel"))
        {
            continueButton.SetActive(true);
        }

    }

    public void Continue()
    {
        playerAbilities.gameObject.SetActive(true);
        playerAbilities.transform.position = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));

        PlayerProgressTracker playerProgress = playerAbilities.GetComponent<PlayerProgressTracker>();

        if (PlayerPrefs.HasKey("DoubleJumpUnlocked"))
        {
            if (PlayerPrefs.GetInt("DoubleJumpUnlocked") == 1)
            {
                playerAbilities.canDoubleJump = true;
            }
        }
        if (PlayerPrefs.HasKey("DashUnlocked"))
        {
            if (PlayerPrefs.GetInt("DashUnlocked") == 1)
            {
                playerAbilities.canDash = true;
            }
        }

        SceneManager.LoadScene(PlayerPrefs.GetString("ContinueLevel"));
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene(newGameScene);
    }

    public void ExitGame()
    {
        Application.Quit();

        Debug.Log("Game Quit");
    }
}
