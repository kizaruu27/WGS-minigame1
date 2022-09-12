using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip speedItemSound;
    public AudioClip stopItemSound;
    public AudioClip obstacleSound;

    public static ObjectSoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void PlaySpeedItemSound()
    {
        audioSource.PlayOneShot(speedItemSound);
    }

    public void PlayStopItemSound()
    {
        audioSource.PlayOneShot(stopItemSound);
    }

    public void PlayObstacleSound()
    {
        audioSource.PlayOneShot(obstacleSound);
    }
}
