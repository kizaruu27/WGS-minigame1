using UnityEngine;
using System;
using Photon.Pun;

public class NPCInfo : MonoBehaviour
{
    [Header("NPC Information")]
    public static NPCInfo instance;
    public int NPCID;
    public string NPCName;
    public int NPCScore;

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
    public void SetPlayerInfo(int newID, string newName) => view.RPC("SetNameNPC", RpcTarget.AllBuffered, newID, newName);

    private void Start() => view.RPC("UpdateNPCName", RpcTarget.AllBuffered, NPCID, NPCName);

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

                // view.RPC("UpdateNPCScore", RpcTarget.AllBuffered, NPCScore, NPCName);
                LeaderboardManager.instance.UpdatePlayerScore(NPCName, NPCScore);

                // Debug.Log("player: "+ playerName + " ngelewatin check poin number: "+ numberOfPassedCheckpoints);

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

    [PunRPC]
    void SetNameNPC(int newID, string newName)
    {
        NPCID = newID;
        NPCName = newName;
    }
}