using UnityEngine;
using System;
using System.Collections;
using Photon.Pun;

public class PlayerInfo : MonoBehaviour
{
    [Header("Player Information")]
    public static PlayerInfo instance;
    public int playerID;
    public string playerName;
    [SerializeField] int playerScore;

    [Header("Check point system")]
    bool isRaceCompleted = false;
    int passedCheckPointNumber = 0;
    int numberOfPassedCheckpoints = 0;
    float timeAtLastPassCheckpoint = 0;
    int lapsCompleted = 0;
    const int lapsToComplete = 1;
    public event Action<PlayerInfo> OnPassCheckpoint;

    float timer = 0f;

    PhotonView view;

    private void Awake()
    {
        instance = this;
        view = GetComponent<PhotonView>();

        playerName = view.Owner.NickName; // nama player 
        playerID = view.Owner.ActorNumber - 1; // ID player 
    }

    private void Start()
    {
        view.RPC("UpdatePlayerName", RpcTarget.AllBuffered, playerID, playerName);
        view.RPC("SetPlayerName", RpcTarget.AllBuffered, playerName);
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
                playerScore++;

                view.RPC(
                    "UpdatePlayerScore", RpcTarget.AllBuffered, //RPC Arguments
                    playerName, playerScore //Method Arguments
                    );

                if (checkpoint.isFinishLine)
                {
                    passedCheckPointNumber = 0;
                    lapsCompleted++;


                    view.RPC(
                        "UpdatePodiumList", RpcTarget.AllBuffered, //RPC Arguments
                        checkpoint.isFinishLine, playerID, timer, playerName //Method Arguments
                        );

                    if (lapsCompleted >= lapsToComplete) // nanti gw edit
                    {
                        isRaceCompleted = true;
                    }
                }

                OnPassCheckpoint?.Invoke(this);
            }


        }


    }


    private void Update() => StartCoroutine(
            CheckAllPlayerConnected.instance.WaitAllPlayerReady(
                () => StartCoroutine(
                    WaitToStart()
                )
            )
        );


    [PunRPC]
    void UpdatePodiumList(bool isFinish, int id, float timer, string playerName)
    {
        GameObject finishUI = GameObject.FindGameObjectWithTag("Finish UI");
        finishUI.GetComponent<MultiplayerFinishManager>().Finish(isFinish, id, timer, playerName);
    }

    [PunRPC]
    void UpdatePlayerScore(string name, int score)
    {
        LeaderboardManager.instance.UpdatePlayerScore(name, score); //disini rpc nya
    }

    [PunRPC]
    void UpdatePlayerName(int id, string name) => LeaderboardManager.instance.UpdatePlayerName(id, name);


    [PunRPC]
    void SetPlayerName(string name) => LeaderboardManager.instance.SetPlayerName(name);

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(3);

        timer += Time.deltaTime;
    }
}