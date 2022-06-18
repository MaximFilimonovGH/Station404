using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgressTracker : MonoBehaviour
{
    public bool[] journalPagesFound = new bool[] { false, false, false, false, false, false, false, false };
    public bool[] doorsOpen = new bool[] { false, false, false, false };
    public bool[] windowsShattered = new bool[] { false, false };

    public bool fingerFound;
    public bool gameFinished;

    public bool[] levelsVisited = new bool[] { false, false, false, false };

    public bool timeToChoose;
}
