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
    public abstract class CharactersInfo : MonoBehaviour
    {
        [Header("Character Information")]
        public static CharactersInfo instance;
        public int CharaID;
        public string CharaName;
        public TMP_Text CharaViewName;
        public int CharaScore;

        [Header("Check point system")]
        bool isRaceCompleted = false;
        int passedCheckPointNumber = 0;
        int numberOfPassedCheckpoints = 0;
        float timeAtLastPassCheckpoint = 0;
        int lapsCompleted = 0;

        protected float timer = 0f;
        protected PhotonView view;
        protected GameManager type;

        FinishLeaderboard FinishUI;

        protected void Awake()
        {
            instance = this;
            view = GetComponent<PhotonView>();
            type = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        protected void Update() => timer += Time.deltaTime;

        protected virtual void OnCollideCheckpoint(Collider coll, Action UpdateScore, Action<GameCheckpoint> UpdatePodium)
        {
            if (coll.CompareTag("Checkpoint"))
            {
                if (isRaceCompleted) return;

                var checkpoint = coll.GetComponent<GameCheckpoint>();
                TryGetComponent(out ICharacterItem myCharacter);

                passedCheckPointNumber = checkpoint.checkPointNumber;
                numberOfPassedCheckpoints++;
                timeAtLastPassCheckpoint = Time.time;
                CharaScore++;

                UpdateScore();

                if (checkpoint.isFinishLine)
                {
                    FinishUI = GameObject
                        .FindGameObjectWithTag("Finish UI")
                        .GetComponent<FinishLeaderboard>();

                    if (myCharacter is Player)
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
            OnCollideCheckpoint(
                other,
                UpdateScore: CheckTypeUpdateScore,
                UpdatePodium: (checkpoint) => CheckTypeUpdatePodium(checkpoint)
            );
        }

        [PunRPC]
        protected void UpdatePodiumList(bool isFinish, int id, float timer, string playerName) =>
            FinishUI.Finish(isFinish, id, timer, playerName);

        [PunRPC]
        protected void UpdatePodiumList(int id, float timer, string playerName) =>
            FinishUI.GetComponent<FinishLeaderboard>().Finish(id, timer, playerName);

        [PunRPC]
        protected void UpdateCharacterScore(string name, int score)
        {
            GameplayLeaderboardManager.instance.UpdatePlayerScore(name, score); //disini rpc nya
        }

        [PunRPC]
        protected void UpdateCharacterName(int id, string name) => GameplayLeaderboardManager.instance.UpdatePlayerName(id, name);

        protected virtual void CheckTypeUpdateScore()
        {
            if (type.IsMultiplayer)
            {
                view.RPC(
                    "UpdateCharacterScore", RpcTarget.AllBuffered, //RPC Arguments
                    CharaName, CharaScore //Method Arguments
                    );
            }
            else
            {
                GameplayLeaderboardManager.instance.UpdatePlayerScore(CharaName, CharaScore);
            }
        }

        protected abstract void CheckTypeUpdatePodium(GameCheckpoint checkpoint);
    }
}