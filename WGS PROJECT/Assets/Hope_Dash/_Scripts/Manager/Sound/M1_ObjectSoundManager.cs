using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class M1_ObjectSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip speedItemSound;
    public AudioClip stopItemSound;
    public AudioClip obstacleSound;

    public static M1_ObjectSoundManager instance;
    private PhotonView pv;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pv = GetComponent<PhotonView>();
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
