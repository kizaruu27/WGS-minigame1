using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class M1_LeaveRoom : MonoBehaviour
{
    public void OnCLickLeaveRoom(string targetScene)
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        SceneManager.LoadScene(targetScene);
    }
}
