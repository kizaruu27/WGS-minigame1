using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using RunMinigames.Mechanics.Characters;
using RunMinigames.Manager.Game;

using System.Linq;

public class M1_NPCSpawner : MonoBehaviour
{
    public static M1_NPCSpawner instance;
    public GameObject[] playerNPCPrefabs;
    public Transform[] spawnPoints;
    [SerializeField] int playerMax;
    [SerializeField] int playerNow;

    public List<int> playerAvatarIndex = new List<int>();
    bool isNPCAlreadySpawned;

    PhotonView view;

    private void Awake()
    {
        playerMax = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerNow = PhotonNetwork.CurrentRoom.PlayerCount;

        view = GetComponent<PhotonView>();
    }

    public void SetPlayerIndex(int index) => playerAvatarIndex.Add(index);
    private void Update() => StartCoroutine(M1_GameManager.instance.WaitAllPlayerReady(SpawnNPC));

    void SpawnNPC()
    {
        List<int> NPCIndex = new List<int> { 0, 1, 2 };

        bool allplayerSpawned = GameObject.FindGameObjectsWithTag("Player").Length == (int)PhotonNetwork.PlayerList.Length;

        if (allplayerSpawned && !isNPCAlreadySpawned)
        {
            List<int> filteredAvatar = NPCIndex.Except(playerAvatarIndex).ToList();

            if (PhotonNetwork.IsMasterClient && !isNPCAlreadySpawned)
            {
                for (int i = 0; i < playerMax; i++)
                {
                    if (i >= playerNow)
                    {
                        Transform spwanPointNPc = spawnPoints[i];
                        string NPCPrefabsName = playerNPCPrefabs[filteredAvatar[Random.Range(0, filteredAvatar.Count)]].name;

                        PhotonNetwork.InstantiateRoomObject(NPCPrefabsName, spwanPointNPc.position, Quaternion.identity);
                        M1_NPCInfoV2.info.SetNPCInfo(i, NPCPrefabsName + " - " + i.ToString());
                    }

                    isNPCAlreadySpawned = i == playerMax - 1;

                    view.RPC(nameof(SyncNPCStatus), RpcTarget.OthersBuffered, isNPCAlreadySpawned);
                }
            }
        }
    }

    [PunRPC]
    void SyncNPCStatus(bool alreadySpawned)
    {
        isNPCAlreadySpawned = alreadySpawned;
    }
}