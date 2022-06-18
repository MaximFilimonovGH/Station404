using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{

    public GameObject[] windows;
    private PlayerProgressTracker playerProgress;

    public int windowNumber;

    // Start is called before the first frame update
    void Start()
    {
        playerProgress = PlayerVitalsController.instance.GetComponent<PlayerProgressTracker>();
        if (playerProgress.windowsShattered[windowNumber])
        {
            foreach (GameObject win in windows)
            {
                win.SetActive(false);
            }
        }

    }
}
