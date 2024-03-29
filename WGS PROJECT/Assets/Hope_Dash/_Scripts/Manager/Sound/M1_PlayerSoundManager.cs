using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M1_PlayerSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footStep;
    public AudioClip jumpSound;

    public static M1_PlayerSoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void PlayFootstepSound()
    {
        audioSource.PlayOneShot(footStep);
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    

    public void StopSound()
    {
        audioSource.Stop();
    }
}
