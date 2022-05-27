using UnityEngine;
using System.Collections;
using Photon.Pun;
using System;


public class CheckAllPlayerConnected : MonoBehaviour
{
    public static CheckAllPlayerConnected instance;
    private void Awake() => instance = this;
    public IEnumerator WaitAllPlayerReady(Action ActionMethod)
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Length == (int)PhotonNetwork.PlayerList.Length);

        ActionMethod();
    }
}