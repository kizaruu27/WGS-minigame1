using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MultiplayerPositionHandler : MonoBehaviour
{
    LeaderboardList LeaderboardList;

    public List<MultiplayerLapCounter> lapCounters = new List<MultiplayerLapCounter>();

    void Awake()
    {
        MultiplayerLapCounter[] lapCounterArray = FindObjectsOfType<MultiplayerLapCounter>();
        lapCounters = lapCounterArray.ToList<MultiplayerLapCounter>();

        foreach (MultiplayerLapCounter lapCounters in lapCounters)
        {
            lapCounters.OnPassCheckpoint += OnPassCheckpoint;
        }

        LeaderboardList = FindObjectOfType<LeaderboardList>();
    }

    void Start()
    {
        LeaderboardList.UpdateList(lapCounters);
    }

    void OnPassCheckpoint(MultiplayerLapCounter lapCounter)
    {
        lapCounters = lapCounters.OrderByDescending(s => s.GetNumberOfCheckpointsPassed()).ThenBy(s => s.GetTimeAtLastCheckpoint()).ToList();

        int playerPosition = lapCounters.IndexOf(lapCounter) + 1;

        lapCounter.setPlayerPosition(playerPosition);

        LeaderboardList.UpdateList(lapCounters);
    }
}
