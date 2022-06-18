using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public AudioSource mainMenuMusic;
    public AudioSource[] levelMusic;
    public AudioSource[] sfx;

    public void PlayMainMenuMusic()
    {
        foreach(AudioSource lvlMusic in levelMusic)
        {
            lvlMusic.Stop();
        }
        mainMenuMusic.Play();
    }

    public void PlayLevelMusic(int levelNumber)
    {
        if (!levelMusic[levelNumber].isPlaying)
        {
            mainMenuMusic.Stop();
            foreach(AudioSource lvlMusic in levelMusic)
            {
                lvlMusic.Stop();
            }

            levelMusic[levelNumber].Play();
        }
    }

    public void PlayBossMusic()
    {
        foreach (AudioSource lvlMusic in levelMusic)
        {
            lvlMusic.Stop();
            mainMenuMusic.Stop();
        }
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }

    public void PlaySFXAdjusted(int sfxToAdjust)
    {
        sfx[sfxToAdjust].pitch = Random.Range(.8f, 1.2f);
        PlaySFX(sfxToAdjust);
    }
}
