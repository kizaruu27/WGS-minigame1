using UnityEngine;
using Photon.Pun;

public class NPCSpawner : MonoBehaviour
{
    public GameObject playerNPCPrefabs;
    public Transform[] spawnPoints;
    [SerializeField] public int playerMax;
    [SerializeField] public int playerNow;

    private void Awake()
    {

        playerMax = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerNow = PhotonNetwork.CurrentRoom.PlayerCount;
        
        if (playerNow < playerMax)
        {
            for (int i = 0; i < playerMax; i++)
            {
                if (i >= playerNow)
                {
                    Transform spwanPointNPc = spawnPoints[i];
                    PhotonNetwork.InstantiateRoomObject(playerNPCPrefabs.name, spwanPointNPc.position, Quaternion.identity);
                    PlayerInfo.instance.SetPlayerInfo(i, playerNPCPrefabs.name + " " + i.ToString());
                }
            }
        }
    }
}
