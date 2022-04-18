using System.Linq;
using System.Collections;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;
    private void Awake()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;

        Transform spawnPoint = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        GameObject playerToSpawn = playerPrefabs[PlayerPrefs.GetInt("playerAvatar")];
        GameObject currentPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);

        PhotonView view = currentPlayer.GetComponent<PhotonView>();
    }


    [PunRPC]
    void SetAvatarIndex(int index)
    {

    }

}
