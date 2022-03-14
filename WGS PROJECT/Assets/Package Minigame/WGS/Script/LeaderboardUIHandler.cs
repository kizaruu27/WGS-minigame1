using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;

    SetLeaderboardItemInfo[] setLeaderboardItemInfo;

    void Awake()
    {
        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        LapCounter[] lapCounterArray = FindObjectsOfType<LapCounter>();

        setLeaderboardItemInfo = new SetLeaderboardItemInfo[lapCounterArray.Length];

        for (int i = 0; i < lapCounterArray.Length; i++)
        {
            GameObject leaderboardInfoGameObject = (Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform));

            setLeaderboardItemInfo[i] = leaderboardInfoGameObject.GetComponent<SetLeaderboardItemInfo>();

            setLeaderboardItemInfo[i].SetPositionText($"{i + 1}.");
        }
    }

    public void UpdateList(List<LapCounter> lapCounters)
    {
        for (int i = 0; i < lapCounters.Count; i++)
        {
            setLeaderboardItemInfo[i].SetPlayerNameText(lapCounters[i].gameObject.name);
        }
    }
}
