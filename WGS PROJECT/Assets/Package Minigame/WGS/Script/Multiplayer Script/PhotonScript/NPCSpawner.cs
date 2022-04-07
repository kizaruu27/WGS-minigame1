using UnityEngine;
using Photon.Pun;

public class NPCSpawner : MonoBehaviour
{
    public GameObject playerNPCPrefabs;
    public Transform[] spawnPoints;
    [SerializeField] int playerMax;
    [SerializeField] int playerNow;
    PhotonView PV;

    private void Awake()
    {
        playerMax = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerNow = PhotonNetwork.CurrentRoom.PlayerCount;
        PV = GetComponent<PhotonView>();

        if (playerNow < playerMax)
        {
            for (int i = 0; i < playerMax; i++)
            {
                if (i >= playerNow)
                {
                    Transform spwanPointNPc = spawnPoints[i];
                    PhotonNetwork.InstantiateRoomObject(playerNPCPrefabs.name, spwanPointNPc.position, Quaternion.identity);
                    // NPCInfo.instance.SetPlayerInfo(i, playerNPCPrefabs.name + " " + i.ToString());
                    PV.RPC("SetNPCName", RpcTarget.AllBuffered, i);
                }
            }
        }
    }

    [PunRPC]
    void SetNPCName(int i){
        NPCInfo.instance.SetPlayerInfo(i, playerNPCPrefabs.name + " " + i.ToString());
    }
}
