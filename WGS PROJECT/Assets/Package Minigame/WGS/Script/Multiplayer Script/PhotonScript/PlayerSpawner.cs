using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerNPCPrefabs;
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;
    public Text[] Texts;
    public PlayerInfo[] players;

    private void Awake()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        
        
        // for (int i=0; i< PhotonNetwork.CurrentRoom.PlayerCount; i++)
        // {
            // Debug.Log("nama player "+ PhotonNetwork.PlayerList[i].NickName);
            
        // }

        Transform spawnPoint = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        GameObject currentPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
        

        // currentPlayer.name = setName;
        // PlayerInfo.instance.SetPlayerInfo(setID);
    }

    private void Start() {
        int setID = PhotonNetwork.LocalPlayer.ActorNumber;
        string setName = PhotonNetwork.LocalPlayer.NickName;

        PlayerInfo[] players = FindObjectsOfType<PlayerInfo>();

        // players[setID].name = PhotonNetwork.PlayerList[0].NickName;
    }
}
