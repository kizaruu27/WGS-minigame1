using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundManager : MonoBehaviour
{
   public AudioSource audioSource;
   public AudioClip buttonClickSound;
   
   public void PlayButtonSound()
   {
      audioSource.PlayOneShot(buttonClickSound);
   }
}
