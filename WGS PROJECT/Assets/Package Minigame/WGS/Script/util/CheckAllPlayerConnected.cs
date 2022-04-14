using UnityEngine;
using System.Collections;
using Photon.Pun;
using System;


public class CheckAllPlayerConnected : MonoBehaviour
{

    public static CheckAllPlayerConnected instance;
    private void Awake() => instance = this;

    public IEnumerator WaitAllPlayerReady(Func<Coroutine> OtherMethod)
    {
        Debug.Log("Player belum lengkap");
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Length == (int)PhotonNetwork.PlayerList.Length);
        Debug.Log("Player Sudah Lengkap");

        OtherMethod();
    }


}