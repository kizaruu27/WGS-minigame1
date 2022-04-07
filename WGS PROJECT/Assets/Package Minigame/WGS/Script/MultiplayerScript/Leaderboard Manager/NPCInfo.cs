using UnityEngine;
using System;
using Photon.Pun;

public class NPCInfo : MonoBehaviour
{
    [Header("Player Information")]
    public static NPCInfo instance;
    public int NPCID;
    public string NPCName;
    public int NPCScore = 100;

    [Header("Check point system")]
    bool isRaceCompleted = false;
    int passedCheckPointNumber = 0;
    int numberOfPassedCheckpoints = 0;
    float timeAtLastPassCheckpoint = 0;
    int lapsCompleted = 0;
    const int lapsToComplete = 1;
    public event Action<NPCInfo> OnPassCheckpoint;

    PhotonView view;

    private void Awake()
    {
        instance = this;
        view = GetComponent<PhotonView>();
    }
    public void SetPlayerInfo(int newID, string newName)
    {
        NPCID = newID;
        NPCName = newName;
        gameObject.name = newName; // ini masih belum rubah
    } //problem nya disini

    private void Start()
    {
        view.RPC("UpdateNPCName", RpcTarget.AllBuffered, NPCID, NPCName);
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

                view.RPC("UpdateNPCScore", RpcTarget.AllBuffered, NPCScore, NPCName);

                Debug.Log(NPCName + " Lewat " + numberOfPassedCheckpoints);

                // Debug.Log("player: "+ playerName + " ngelewatin check poin number: "+ numberOfPassedCheckpoints);

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
    void UpdateNPCScore(int score, string name)
    {
        LeaderboardManager.instance.UpdatePlayerScore(name, score);
    }

    [PunRPC]
    void UpdateNPCName(int id, string name) => LeaderboardManager.instance.UpdatePlayerName(id, name);
}
