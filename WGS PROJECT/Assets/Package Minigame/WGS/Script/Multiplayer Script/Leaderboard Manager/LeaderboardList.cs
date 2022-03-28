using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardList : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;

    LeaderboardItem[] setLeaderboardItemInfo;

    void Awake()
    {
        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        MultiplayerLapCounter[] lapCounterArray = FindObjectsOfType<MultiplayerLapCounter>();

        setLeaderboardItemInfo = new LeaderboardItem[lapCounterArray.Length];

        for (int i = 0; i < lapCounterArray.Length; i++)
        {
            GameObject leaderboardInfoGameObject = (Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform));

            setLeaderboardItemInfo[i] = leaderboardInfoGameObject.GetComponent<LeaderboardItem>();

            setLeaderboardItemInfo[i].SetPositionText($"{i + 1}.");
        }
    }

    public void UpdateList(List<MultiplayerLapCounter> lapCounters)
    {
        for (int i = 0; i < lapCounters.Count; i++)
        {
            setLeaderboardItemInfo[i].SetPlayerName(lapCounters[i].PlayerName);
            Debug.Log("masuk ke fungsi setName: "+ lapCounters[i].PlayerName);
        }
    }

}
