
using UnityEngine;
using Photon.Pun;

public class NPCSpawner : MonoBehaviour
{
    public GameObject playerNPCPrefabs;
    public Transform[] spawnPoints;

    private void Awake()
    {
        int playerMax = PhotonNetwork.CurrentRoom.MaxPlayers;
        int playerNow = PhotonNetwork.CurrentRoom.PlayerCount;

        if (playerNow <= playerMax)
        {
            for (int i = 0; i < playerMax; i++)
            {
                if (i >= playerNow)
                {
                    Transform spwanPointNPc = spawnPoints[i];
                    Instantiate(playerNPCPrefabs, spwanPointNPc.position, Quaternion.identity);
                    MultiplayerLapCounter.instance.SetGameObjectName(playerNPCPrefabs.name + " " + i.ToString());
                    LeaderboardList.instace.addNPCName(playerNPCPrefabs, i);
                }
            }
        }
    }
}
