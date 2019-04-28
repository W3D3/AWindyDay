using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource wind;
    public AudioClip windClip;
    public AudioSource win;
    public AudioClip winClip;
    public AudioSource defeat;
    public AudioClip defeatClip;

    public static SoundManager Instance = null;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void playWind()
    {
        if (!wind.isPlaying)
        {
            wind.clip = windClip;
            wind.Play();
        }
    }

    public void playWin()
    {
        win.clip = winClip;
        win.Play();
    }
    
    public void playLose()
    {
        defeat.clip = defeatClip;
        defeat.Play();
    }
}
