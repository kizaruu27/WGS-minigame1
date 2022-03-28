using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class SystemSementara : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] GameObject leaderboardItemPrefab;

    private void Start() {
        foreach(Player player in PhotonNetwork.PlayerList){
            AddScoreboardItem(player);
        }
    }

    void AddScoreboardItem(Player player){
        LeaderboardItem item = Instantiate(leaderboardItemPrefab, container).GetComponent<LeaderboardItem>();
        item.SetPlayerName(player);
    }
}
