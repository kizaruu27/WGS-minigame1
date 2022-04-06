using System.Linq;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerNPCPrefabs;
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    private void Awake()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;

        Transform spawnPoint = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        GameObject currentPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
        // Debug.Log(PlayerPrefs.GetString("PLAYERNICKNAME")); 


        int setID = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        string setName = PhotonNetwork.LocalPlayer.NickName;
        currentPlayer.name = setName + " " + setID.ToString();
        PlayerInfo.instance.SetPlayerInfo(setID, setName);
    }
}
