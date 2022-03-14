using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PositionHandler : MonoBehaviour
{
    public List<LapCounter> lapCounters = new List<LapCounter>();

    void Start()
    {
        LapCounter[] lapCounterArray = FindObjectsOfType<LapCounter>();

        lapCounters = lapCounterArray.ToList<LapCounter>();

        foreach (LapCounter lapCounters in lapCounters)
        {
            lapCounters.OnPassCheckpoint += OnPassCheckpoint;
        }
    }

    void OnPassCheckpoint(LapCounter lapCounter)
    {
        //Debug.Log($"Event: Player {lapCounter.gameObject.name} passed a checkpoint");

        lapCounters = lapCounters.OrderByDescending(s => s.GetNumberOfCheckpointsPassed()).ThenBy(s => s.GetTimeAtLastCheckpoint()).ToList();

        int playerPosition = lapCounters.IndexOf(lapCounter) + 1;

        lapCounter.setPlayerPosition(playerPosition);
    }
}
