using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class audioManager : MonoBehaviour
{

    public static audioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("MainSpaceTheme");
    }


    public void PlayMusic(string nameClip)
    {
        Sound s = Array.Find(musicSounds, Matrix4x4 => Matrix4x4.nameClip == nameClip);
        if (s == null)
        {
            Debug.Log("No sound found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string nameClip)
    {
        Sound s = Array.Find(sfxSounds, Matrix4x4 => Matrix4x4.nameClip == nameClip);
        if (s == null)
        {
            Debug.Log("No sound found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }






}
