using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInfo : MonoBehaviour
{
    [Header("Player Information")]
    public static PlayerInfo instance;
    public int playerID;
    public int playerScore = 100;

    [Header("Check point system")]
    bool isRaceCompleted = false;
    int passedCheckPointNumber = 0;
    int numberOfPassedCheckpoints = 0;
    float timeAtLastPassCheckpoint = 0;
    int lapsCompleted = 0;
    const int lapsToComplete = 1;
    public event Action <PlayerInfo> OnPassCheckpoint;

    private void Awake() => instance = this;
    public void SetPlayerInfo(int newID, string newName){
        playerID = newID;
        gameObject.name = newName;
    }
    public void SetPlayerInfo(int newID){
        playerID = newID;
    }

    private void Start() {
        LeaderboardManager.instance.UpdatePlayerName(playerID, gameObject.name);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Checkpoint"))
        {
            
            if (isRaceCompleted)
            {
                return;
            }

            Checkpoint checkpoint = coll.GetComponent<Checkpoint>();

            if (passedCheckPointNumber + 1 == checkpoint.checkPointNumber)
            {
                

                passedCheckPointNumber = checkpoint.checkPointNumber;
                numberOfPassedCheckpoints++;
                timeAtLastPassCheckpoint = Time.time;
                LeaderboardManager.instance.UpdatePlayerScore(gameObject.name, playerScore);

                // Debug.Log("player: "+ gameObject.name + " ngelewatin check poin number: "+ numberOfPassedCheckpoints);

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
