using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class LeaderboardList : MonoBehaviour
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
        item.Initialize(player);
    }
}
