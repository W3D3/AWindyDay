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

    public static SoundManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = null;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void playWind()
    {
        wind.clip = windClip;
        wind.Play();
    }

    public void playWin()
    {
        win.clip = winClip;
    }
}
