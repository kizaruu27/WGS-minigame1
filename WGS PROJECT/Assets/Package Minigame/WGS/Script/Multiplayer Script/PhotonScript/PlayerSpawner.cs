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

        Transform spawnPoint = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber-1];
        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        GameObject currentPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
        // Debug.Log(PlayerPrefs.GetString("PLAYERNICKNAME")); 
        // currentPlayer.name = PhotonNetwork.NickName;

        MultiplayerLapCounter.instance.SetGameObjectName(PhotonNetwork.NickName);

        int playerMax = PhotonNetwork.CurrentRoom.MaxPlayers;
        int playerNow = PhotonNetwork.CurrentRoom.PlayerCount;

        if (playerNow <= playerMax){
            for(int i = 0; i < playerMax; i++) {
                if(i >= playerNow ){
                    Transform spwanPointNPc = spawnPoints[i];
                    PhotonNetwork.Instantiate(playerNPCPrefabs.name, spwanPointNPc.position, Quaternion.identity);
                }
            }
        }
    }
}
