using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LeaderboardList : MonoBehaviour
{
    public static LeaderboardList instace;
    public GameObject leaderboardItemPrefab;
    [SerializeField] Transform container;

    [SerializeField] LeaderboardItem[] setLeaderboardItemInfo; // gak kedetect
    // *test
    [SerializeField] MultiplayerLapCounter[] lapCounterArray; // gak kedetect

    private void Awake() => instace = this;
    
    private void Start() {

        // int totalPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
        
        // Debug.Log($"total player: {totalPlayer}");

        // for (int i = 0; i < totalPlayer; i++)
        // {

        //     LeaderboardItem leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, container).GetComponent<LeaderboardItem>();

        //     leaderboardInfoGameObject.SetPositionText($"{i + 1}.");

            // setLeaderboardItemInfo[i] = leaderboardInfoGameObject.GetComponent<LeaderboardItem>();

            // setLeaderboardItemInfo[i].SetPositionText($"{i + 1}.");
        // }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            addPlayerName(p);
        }
    }

    public void addPlayerName(Player p){
        LeaderboardItem leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, container).GetComponent<LeaderboardItem>();

        leaderboardInfoGameObject.SetPlayerName(p.NickName);
    }


    public void UpdateList(List<MultiplayerLapCounter> lapCounters)
    {
        for (int i = 0; i < lapCounters.Count; i++)
        {
            // setLeaderboardItemInfo[i].SetPlayerName(lapCounters[i].gameObject.name);
            
        }
    }

}
