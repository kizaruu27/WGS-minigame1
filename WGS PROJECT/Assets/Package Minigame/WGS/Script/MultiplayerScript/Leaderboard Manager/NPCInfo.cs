using UnityEngine;
using System;
using Photon.Pun;
using System.Collections;
using TMPro;

public class NPCInfo : MonoBehaviour
{
    [Header("NPC Information")]
    public static NPCInfo instance;
    public int NPCID;
    public string NPCName;
    public TMP_Text characterName; 
    [SerializeField] int NPCScore;

    [Header("Check point system")]
    bool isRaceCompleted = false;
    int passedCheckPointNumber = 0;
    int numberOfPassedCheckpoints = 0;
    float timeAtLastPassCheckpoint = 0;
    const int lapsToComplete = 1;
    public event Action<NPCInfo> OnPassCheckpoint;

    float timer = 0f;
    PhotonView view;

    private void Awake()
    {
        instance = this;
        view = GetComponent<PhotonView>();
        
    }
    public void SetPlayerInfo(int newID, string newName) => view.RPC("SetNameNPC", RpcTarget.AllBuffered, newID, newName);

    private void Start() {
        view.RPC("UpdateNPCName", RpcTarget.AllBuffered, NPCID, NPCName);
        characterName.text = NPCName;
    } 

    private void Update()
    {
        StartCoroutine(CheckAllPlayerConnected.instance.WaitAllPlayerReady(() => StartCoroutine(WaitToStart())));
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
            Multiplayer_NPCRun myNpc = GetComponent<Multiplayer_NPCRun>();

            if (passedCheckPointNumber + 1 == checkpoint.checkPointNumber)
            {
                passedCheckPointNumber = checkpoint.checkPointNumber;
                numberOfPassedCheckpoints++;
                timeAtLastPassCheckpoint = Time.time;
                NPCScore++;

                view.RPC("UpdateNPCScore", RpcTarget.AllBuffered, NPCScore, NPCName);

                if (checkpoint.isFinishLine)
                {
                    view.RPC("UpdatePodiumList", RpcTarget.AllBuffered, NPCID, timer, NPCName);
                    myNpc.MaxPlayerSpeed = 2;
                }

                OnPassCheckpoint?.Invoke(this);
            }

            if(checkpoint.stopAfterFinish == true) myNpc.NPCCanMove = false;
        }
    }

    [PunRPC]
    void UpdatePodiumList(int id, float timer, string playerName)
    {
        GameObject ds = GameObject.FindGameObjectWithTag("Finish UI");
        ds.GetComponent<MultiplayerFinishManager>().Finish(id, timer, playerName);
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

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(4);

        timer += Time.deltaTime;
    }
}