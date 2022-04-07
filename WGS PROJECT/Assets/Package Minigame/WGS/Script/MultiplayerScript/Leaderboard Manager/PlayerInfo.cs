using UnityEngine;
using System;
using Photon.Pun;

public class PlayerInfo : MonoBehaviour
{
    [Header("Player Information")]
    public static PlayerInfo instance;
    public int playerID;
    public string playerName;
    public int playerScore;

    [Header("Check point system")]
    bool isRaceCompleted = false;
    int passedCheckPointNumber = 0;
    int numberOfPassedCheckpoints = 0;
    float timeAtLastPassCheckpoint = 0;
    int lapsCompleted = 0;
    const int lapsToComplete = 1;
    public event Action<PlayerInfo> OnPassCheckpoint;

    PhotonView view;

    private void Awake()
    {
        instance = this;
        view = GetComponent<PhotonView>();

        playerName = view.Owner.NickName; // nama player 
        playerID = view.Owner.ActorNumber - 1; // ID player 
    }

    private void Start() => view.RPC("UpdatePlayerName", RpcTarget.AllBuffered, playerID, playerName);

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

                // view.RPC("UpdatePlayerScore", RpcTarget.AllBuffered, playerName, playerScore);
                LeaderboardManager.instance.UpdatePlayerScore(playerName, playerScore);
                

                Debug.Log(playerName);


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

    [PunRPC]
    void UpdatePlayerScore(string name, int score)
    {
        LeaderboardManager.instance.UpdatePlayerScore(name, score); //disini rpc nya
    }

    [PunRPC]
    void UpdatePlayerName(int id, string name) => LeaderboardManager.instance.UpdatePlayerName(id, name);
}