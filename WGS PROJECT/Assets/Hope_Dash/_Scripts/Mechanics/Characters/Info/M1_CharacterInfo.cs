using UnityEngine;
using System;
using Photon.Pun;
using RunMinigames.Interface;
using RunMinigames.Manager.Characters;
using RunMinigames.Mechanics.Checkpoint;
using RunMinigames.Manager.Leaderboard;
using TMPro;
using RunMinigames.Manager.Game;

namespace RunMinigames.Mechanics.Characters
{
  public abstract class M1_CharactersInfo : MonoBehaviour
  {
    [Header("Character Information")]
    public static M1_CharactersInfo instance;
    public int CharaID;
    public string CharaName;
    public TMP_Text CharaViewName;
    public float CharaScore;

    [Header("Check point system")]
    int passedCheckPointNumber = 0;
    int lapsCompleted = 0;
    public bool isPlayerFinish;

    protected float timer = 0f;
    protected PhotonView view;
    protected M1_GameManager type;

    M1_FinishLeaderboard _m1FinishUI;

    protected void Awake()
    {
      instance = this;
      view = GetComponent<PhotonView>();
      type = GameObject.Find("GameManager").GetComponent<M1_GameManager>();

      _m1FinishUI = GameObject
          .FindGameObjectWithTag("Finish UI")
          .GetComponent<M1_FinishLeaderboard>();
    }

    protected void Update() => timer += Time.deltaTime;

    protected virtual void OnCollideCheckpoint(Collider coll, Action UpdateScore, Action<M1_GameCheckpoint> UpdatePodium)
    {
      if (coll.CompareTag("Checkpoint"))
      {

        var checkpoint = coll.GetComponent<M1_GameCheckpoint>();
        TryGetComponent(out M1_ICharacterItem myCharacter);

        passedCheckPointNumber = checkpoint.checkPointNumber;

        if (!checkpoint.isFinishLine && !checkpoint.stopAfterFinish)
          CharaScore = checkpoint.checkPointNumber;

        UpdateScore();

        if (checkpoint.isFinishLine)
        {
          isPlayerFinish = checkpoint.isFinishLine;

          if (myCharacter is M1M1Player)
          {
            passedCheckPointNumber = 0;
            lapsCompleted++;
          }

          UpdatePodium(checkpoint);
          myCharacter.MaxSpeed = 2;
        }

        if (checkpoint.stopAfterFinish) myCharacter.CanMove = false;
      }
    }

    protected void OnTriggerEnter(Collider other)
    {
      if (view.IsMine)
      {
        OnCollideCheckpoint(
            other,
            UpdateScore: CheckTypeUpdateScore,
            UpdatePodium: (checkpoint) => CheckTypeUpdatePodium(checkpoint)
        );
      }
    }

    [PunRPC]
    protected void UpdatePodiumList(bool isFinish, int id, float timer, string playerName) =>
        _m1FinishUI.Finish(isFinish, id, timer, playerName);

    [PunRPC]
    protected void UpdatePodiumList(int id, float timer, string playerName) =>
        _m1FinishUI.Finish(id, timer, playerName);

    [PunRPC]
    protected void UpdateCharacterScore(string name, float score)
    {
      M1_GameplayLeaderboardManager.instance.UpdatePlayerScore(name, score); //disini rpc nya
    }

    [PunRPC]
    protected void UpdateCharacterName(int id, string name) => M1_GameplayLeaderboardManager.instance.UpdatePlayerName(id, name);

    protected virtual void CheckTypeUpdateScore()
    {
      if (type.IsMultiplayer)
      {
        if (view.IsMine)
        {
          view.RPC(
              nameof(UpdateCharacterScore), RpcTarget.AllBuffered, //RPC Arguments
              CharaName, CharaScore //Method Arguments
              );
        }
      }
      else
      {
        M1_GameplayLeaderboardManager.instance.UpdatePlayerScore(CharaName, CharaScore);
      }
    }

    protected abstract void CheckTypeUpdatePodium(M1_GameCheckpoint checkpoint);
  }
}