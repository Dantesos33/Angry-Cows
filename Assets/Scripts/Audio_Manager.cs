using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    [Header("-------Audio Source--------")]
    [SerializeField] AudioSource musicSource;

    [Header("-------Audio Clip--------")]
    public AudioClip cowChase;

    public void PlayMusic(AudioClip audioClip)
    {
        musicSource.PlayOneShot(audioClip);
    }
}
