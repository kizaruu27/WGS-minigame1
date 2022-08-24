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
        Transform spawnPoint = spawnPoints[PlayerPrefs.GetInt("positionIndex")];
        GameObject playerToSpawn = playerPrefabs[PlayerPrefs.GetInt("playerAvatar")];
        GameObject currentPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
    }

    private void Update()
    {
        Debug.Log(PlayerPrefs.GetInt("playerAvatar"));
    }

}
