using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PositionHandler : MonoBehaviour
{
    LeaderboardUIHandler leaderboardUIHandler;

    public List<LapCounter> lapCounters = new List<LapCounter>();

    void Awake()
    {
        LapCounter[] lapCounterArray = FindObjectsOfType<LapCounter>();
        lapCounters = lapCounterArray.ToList<LapCounter>();

        foreach (LapCounter lapCounters in lapCounters)
        {
            lapCounters.OnPassCheckpoint += OnPassCheckpoint;
        }

        leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();
    }

    void Start()
    {
        leaderboardUIHandler.UpdateList(lapCounters);
    }

    void OnPassCheckpoint(LapCounter lapCounter)
    {
        lapCounters = lapCounters.OrderByDescending(s => s.GetNumberOfCheckpointsPassed()).ThenBy(s => s.GetTimeAtLastCheckpoint()).ToList();

        int playerPosition = lapCounters.IndexOf(lapCounter) + 1;

        lapCounter.setPlayerPosition(playerPosition);

        leaderboardUIHandler.UpdateList(lapCounters);
    }
}
