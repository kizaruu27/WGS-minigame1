using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LeaderboardList : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;
    [SerializeField] Transform container;

    [SerializeField] LeaderboardItem[] setLeaderboardItemInfo;


    // *test
    [SerializeField] MultiplayerLapCounter[] lapCounterArray;

    void Start()
    {

        lapCounterArray = GameObject.FindObjectsOfType<MultiplayerLapCounter>();

        setLeaderboardItemInfo = new LeaderboardItem[lapCounterArray.Length];

        Debug.Log("jumlah orang: "+ lapCounterArray.Length);
        

        for (int i = 0; i < lapCounterArray.Length; i++)
        {

            LeaderboardItem leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, container).GetComponent<LeaderboardItem>();

            setLeaderboardItemInfo[i] = leaderboardInfoGameObject.GetComponent<LeaderboardItem>();

            setLeaderboardItemInfo[i].SetPositionText($"{i + 1}.");
        }
    }


    public void UpdateList(List<MultiplayerLapCounter> lapCounters)
    {
        for (int i = 0; i < lapCounters.Count; i++)
        {
            setLeaderboardItemInfo[i].SetPlayerName(lapCounters[i].PlayerName);
            
        }
    }

}
