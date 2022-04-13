using UnityEngine;
using Photon.Pun;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] playerNPCPrefabs;
    public Transform[] spawnPoints;
    [SerializeField] int playerMax;
    [SerializeField] int playerNow;

    private void Awake()
    {
        playerMax = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerNow = PhotonNetwork.CurrentRoom.PlayerCount;

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < playerMax; i++)
            {
                if (i >= playerNow)
                {
                    Transform spwanPointNPc = spawnPoints[i];
                    string NPCPrefabsName = playerNPCPrefabs[Random.Range(0, playerNPCPrefabs.Length)].name;

                    PhotonNetwork.InstantiateRoomObject(NPCPrefabsName, spwanPointNPc.position, Quaternion.identity);
                    NPCInfo.instance.SetPlayerInfo(i, NPCPrefabsName + " - " + i.ToString());
                }
            }
        }
    }
}