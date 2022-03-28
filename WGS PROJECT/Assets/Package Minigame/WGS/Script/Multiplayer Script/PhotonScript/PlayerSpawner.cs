using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    private void Start()
    {

        PhotonNetwork.CurrentRoom.IsOpen = false;

        Transform spawnPoint = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber];
        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        GameObject currentPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
        // Debug.Log(PlayerPrefs.GetString("PLAYERNICKNAME")); 
        currentPlayer.name = PlayerPrefs.GetString("PLAYERNICKNAME");
    }
}
