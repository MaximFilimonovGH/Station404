using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{

    public void Start()
    {
        // freeze time
        Time.timeScale = 0f;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
