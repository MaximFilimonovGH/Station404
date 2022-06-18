using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string newGameScene;

    public GameObject continueButton;

    public PlayerAbilityTracker playerAbilities;
    public PlayerProgressTracker playerProgress;

    // Start is called before the first frame update
    void Start()
    {
        // check if there is any saved progress
        if(PlayerPrefs.HasKey("ContinueLevel"))
        {
            continueButton.SetActive(true);
        }

        AudioManager.instance.PlayMainMenuMusic();
    }

    public void Continue()
    {
        playerAbilities.gameObject.SetActive(true);
        playerAbilities.transform.position = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));

        // load ability unlock progress
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

        // load game progress for journal pages
        if (PlayerPrefs.HasKey("JournalPageFound_0"))
        {
            if (PlayerPrefs.GetInt("JournalPageFound_0") == 1)
            {
                //playerProgress.zeroJournalPageFound = true;
                playerProgress.journalPagesFound[0] = true;
            }
        }
        if (PlayerPrefs.HasKey("JournalPageFound_1"))
        {
            if (PlayerPrefs.GetInt("JournalPageFound_1") == 1)
            {
                playerProgress.journalPagesFound[1] = true;
            }
        }
        if (PlayerPrefs.HasKey("JournalPageFound_2"))
        {
            if (PlayerPrefs.GetInt("JournalPageFound_2") == 1)
            {
                playerProgress.journalPagesFound[2] = true;
            }
        }
        if (PlayerPrefs.HasKey("JournalPageFound_3"))
        {
            if (PlayerPrefs.GetInt("JournalPageFound_3") == 1)
            {
                playerProgress.journalPagesFound[3] = true;
            }
        }
        if (PlayerPrefs.HasKey("JournalPageFound_4"))
        {
            if (PlayerPrefs.GetInt("JournalPageFound_4") == 1)
            {
                playerProgress.journalPagesFound[4] = true;
            }
        }

        // load game progress for door unlocks
        if (PlayerPrefs.HasKey("DoorOpen_0"))
        {
            if (PlayerPrefs.GetInt("DoorOpen_0") == 1)
            {
                playerProgress.doorsOpen[0] = true;
            }
        }
        if (PlayerPrefs.HasKey("DoorOpen_1"))
        {
            if (PlayerPrefs.GetInt("DoorOpen_1") == 1)
            {
                playerProgress.doorsOpen[1] = true;
            }
        }
        if (PlayerPrefs.HasKey("DoorOpen_2"))
        {
            if (PlayerPrefs.GetInt("DoorOpen_2") == 1)
            {
                playerProgress.doorsOpen[2] = true;
            }
        }
        if (PlayerPrefs.HasKey("DoorOpen_3"))
        {
            if (PlayerPrefs.GetInt("DoorOpen_3") == 1)
            {
                playerProgress.doorsOpen[3] = true;
            }
        }

        // shattered windows
        if (PlayerPrefs.HasKey("WindowShattered_0"))
        {
            if (PlayerPrefs.GetInt("WindowShattered_0") == 1)
            {
                playerProgress.windowsShattered[0] = true;
            }
        }

        // finger
        if (PlayerPrefs.HasKey("FingerFound"))
        {
            if (PlayerPrefs.GetInt("FingerFound") == 1)
            {
                playerProgress.fingerFound = true;
            }
        }

        // levels visited
        if (PlayerPrefs.HasKey("LevelVisited_0"))
        {
            if (PlayerPrefs.GetInt("LevelVisited_0") == 1)
            {
                playerProgress.levelsVisited[0] = true;
            }
        }
        if (PlayerPrefs.HasKey("LevelVisited_1"))
        {
            if (PlayerPrefs.GetInt("LevelVisited_1") == 1)
            {
                playerProgress.levelsVisited[1] = true;
            }
        }
        if (PlayerPrefs.HasKey("LevelVisited_2"))
        {
            if (PlayerPrefs.GetInt("LevelVisited_2") == 1)
            {
                playerProgress.levelsVisited[2] = true;
            }
        }
        if (PlayerPrefs.HasKey("LevelVisited_3"))
        {
            if (PlayerPrefs.GetInt("LevelVisited_3") == 1)
            {
                playerProgress.levelsVisited[3] = true;
            }
        }



        SceneManager.LoadScene(PlayerPrefs.GetString("ContinueLevel"));
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        playerProgress.levelsVisited[0] = false;

        SceneManager.LoadScene(newGameScene);
    }

    public void ExitGame()
    {
        Application.Quit();

        Debug.Log("Game Quit");
    }
}
