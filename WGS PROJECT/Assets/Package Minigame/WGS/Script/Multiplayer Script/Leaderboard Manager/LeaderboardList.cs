using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class LeaderboardList : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] GameObject leaderboardItemPrefab;
    string[] LeaderboardItem = new string[4];
    int countPlayersOnline;

    string checkName;

    private void Awake() {


        // VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        // MultiplayerLapCounter[] lapCounterArray = FindObjectsOfType<MultiplayerLapCounter>();

        // LeaderboardItem = new LeaderboardItem[lapCounterArray.Length];

        // for (int i = 0; i < lapCounterArray.Length; i++)
        // {
        //     GameObject leaderboardInfoGameObject = (Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform));

        //     LeaderboardItem[i] = leaderboardInfoGameObject.GetComponent<LeaderboardItem>();

        //     // LeaderboardItem[i] = Instantiate(leaderboardItemPrefab, container).GetComponent<LeaderboardItem>();

        //     LeaderboardItem[i].SetPlayerRank($"{i + 1}.");
        // }

        
        // check player yang masuk ke room
        
        countPlayersOnline = PhotonNetwork.CountOfPlayers;
        print(countPlayersOnline+ " player online");

        // print("di LeaderboardList: "+checkName);

    }

    // public void UpdateList(List<MultiplayerLapCounter> lapCounters)
    // {
    //         for (int i = 0; i < lapCounters.Count; i++)
    //         {
    //             // LeaderboardItem[i].SetPlayerName(lapCounters[i].PlayerName);

    //             checkName = lapCounters[i].PlayerName;
    //         }
    // }

    
    // * sementara * 

    private void Start() {
        foreach(Player player in PhotonNetwork.PlayerList){
            AddScoreboardItem(player);
            // CollectAllPlayerOnArray(player);
        }

        // for (int i = 0; i < LeaderboardItem.Length; i++){
        //     if(LeaderboardItem[i] != null){
        //         print(LeaderboardItem[i]);
        //     }
        // }
    }

    void AddScoreboardItem(Player player){
        LeaderboardItem item = Instantiate(leaderboardItemPrefab, container).GetComponent<LeaderboardItem>();
        item.SetPlayerName(player);
    }

    // void CollectAllPlayerOnArray(Player player){
        
    //     for (int i = 0; i < PhotonNetwork.CountOfPlayers; i++){
    //         LeaderboardItem[i] = player.NickName;
    //     }
    // }   
}
