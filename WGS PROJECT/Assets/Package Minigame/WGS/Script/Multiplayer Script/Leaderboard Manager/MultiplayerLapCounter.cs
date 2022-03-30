using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MultiplayerLapCounter : MonoBehaviour
{
    public static MultiplayerLapCounter instance;
    int passedCheckPointNumber = 0;
    float timeAtLastPassCheckpoint = 0;
    int numberOfPassedCheckpoints = 0;
    int lapsCompleted = 0;
    const int lapsToComplete = 1;
    bool isRaceCompleted = false;
    public int playerPosition = 0; // buat podium

    public string PlayerName;
    
    public event Action <MultiplayerLapCounter> OnPassCheckpoint;

    private void Awake() {
        gameObject.name = PlayerName;

        instance = this;
    }

    public void SetGameObjectName(string newName) => gameObject.name = newName;

    public void setPlayerPosition(int position)
    {
        playerPosition = position;
    }

    public int GetNumberOfCheckpointsPassed()
    {
        return numberOfPassedCheckpoints;
    }

    public float GetTimeAtLastCheckpoint()
    {
        return timeAtLastPassCheckpoint;
    }


    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Checkpoint"))
        {
            // Debug.Log("masuk ke lapcounter: "+ PlayerPrefs.GetString("PLAYERNICKNAME"));
            

            if (isRaceCompleted)
            {
                return;
            }

            Checkpoint checkpoint = coll.GetComponent<Checkpoint>();

            if (passedCheckPointNumber + 1 == checkpoint.checkPointNumber)
            {
                Debug.Log("player: "+ gameObject.name + " ngelewatin checkpoin ke: "+ passedCheckPointNumber);

                passedCheckPointNumber = checkpoint.checkPointNumber;
                numberOfPassedCheckpoints++;
                timeAtLastPassCheckpoint = Time.time;

                if (checkpoint.isFinishLine)
                {
                    passedCheckPointNumber = 0;
                    lapsCompleted++;

                    if (lapsCompleted >= lapsToComplete) // nanti gw edit
                    {
                        isRaceCompleted = true;
                    }
                }

                OnPassCheckpoint?.Invoke(this);
            }
        }
    }
}
