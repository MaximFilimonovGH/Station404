using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    public AudioManager theAM;

    private void Awake()
    {
        if (AudioManager.instance == null)
        {
            AudioManager newAM = Instantiate(theAM);
            AudioManager instace = theAM;
            DontDestroyOnLoad(newAM.gameObject);
        }
    }

}
