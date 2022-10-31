using UnityEngine;
using System.Collections;
using Photon.Pun;
using System;


public class M1_CheckAllPlayerConnected : MonoBehaviour
{
    public static M1_CheckAllPlayerConnected instance;
    private void Awake() => instance = this;
    public IEnumerator WaitAllPlayerReady(Action ActionMethod)
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Length == (int)PhotonNetwork.PlayerList.Length);

        ActionMethod();
    }
}