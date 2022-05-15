using UnityEngine;
using System;
using RunMinigames.Mechanics.Checkpoint;

public class LapCounter : MonoBehaviour
{
    //public Text playerPositionTxt;
    int passedCheckPointNumber = 0;
    float timeAtLastPassCheckpoint = 0;
    int numberOfPassedCheckpoints = 0;
    int lapsCompleted = 0;
    const int lapsToComplete = 1;
    bool isRaceCompleted = false;
    public int playerPosition = 0; // buat podium

    public event Action<LapCounter> OnPassCheckpoint;

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

    // IEnumerator ShowPosition(float delayUntilHidePosition)
    // {
    //     playerPositionTxt.text = playerPosition.ToString();
    //     playerPositionTxt.gameObject.SetActive(true);

    //     yield return new WaitForSeconds(delayUntilHidePosition);

    //     playerPositionTxt.gameObject.SetActive(false);
    // }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Checkpoint"))
        {
            if (isRaceCompleted)
            {
                return;
            }

            var checkpoint = coll.GetComponent<GameCheckpoint>();

            if (passedCheckPointNumber + 1 == checkpoint.checkPointNumber)
            {
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

                // if (isRaceCompleted)
                // {
                //     StartCoroutine(ShowPosition(100));
                // }
                // else
                // {
                //     StartCoroutine(ShowPosition(1.5f));
                // }
            }
        }
    }
}
