using UnityEngine;
using System.Collections;

public class Coutdown : MonoBehaviour
{
    public void PlayCountdown()
    {
        StartCoroutine(CheckAllPlayerConnected.instance.WaitAllPlayerReady(() => StartCoroutine(WaitToStart())));
    }

    IEnumerator WaitToStart()
    {
        gameObject.SetActive(false);

        yield return new WaitForSeconds(0);
    }
}