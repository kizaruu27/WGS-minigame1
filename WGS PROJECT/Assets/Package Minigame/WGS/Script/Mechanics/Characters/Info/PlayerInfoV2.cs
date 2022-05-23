using UnityEngine;
using Photon.Pun;
using RunMinigames.Mechanics.Checkpoint;
using RunMinigames.Manager.Leaderboard;

namespace RunMinigames.Mechanics.Characters
{
    public class PlayerInfoV2 : CharactersInfo
    {
        private new void Awake()
        {
            base.Awake();

            if (type.IsMultiplayer && view.IsMine)
            {
                GameObject
                    .FindGameObjectWithTag("Camera")
                    .GetComponent<RunMinigames.Mechanics.Cam.CameraController>().Player = transform;
            }

            CharaName = type.IsMultiplayer && !type.IsSingleplayer ? view.Owner.NickName : gameObject.name;
            CharaID = type.IsMultiplayer && !type.IsSingleplayer ? view.Owner.ActorNumber - 1 : 0;

            if (type.IsMultiplayer)
            {
                view.RPC("SetAvatarIndex", RpcTarget.AllBuffered, PlayerPrefs.GetInt("playerAvatar"));
            }

            CharaViewName.text = CharaName;
        }

        private void Start()
        {
            if (type.IsMultiplayer && !type.IsSingleplayer)
            {
                view.RPC("UpdateCharacterName", RpcTarget.AllBuffered, CharaID, CharaName);
                view.RPC("SetPlayerName", RpcTarget.AllBuffered, CharaName);
            }

            GameplayLeaderboardManager.instance.UpdatePlayerName(CharaID, CharaName);
        }

        protected override void CheckTypeUpdatePodium(GameCheckpoint checkpoint)
        {
            if (type.IsMultiplayer && !type.IsSingleplayer)
            {
                view.RPC(
                    "UpdatePodiumList", RpcTarget.AllBuffered, //RPC Arguments
                    checkpoint.isFinishLine, CharaID, timer, CharaName //Method Arguments
                    );
            }
            else
            {
                GameObject finishUI = GameObject.FindGameObjectWithTag("Finish UI");
                finishUI.GetComponent<FinishLeaderboard>()
                    .Finish(checkpoint.isFinishLine, CharaID, timer, CharaName);
            }
        }

        [PunRPC]
        void SetPlayerName(string name) => GameplayLeaderboardManager.instance.SetPlayerName(name);


        [PunRPC]
        void SetAvatarIndex(int index)
        {
            GameObject manager = GameObject.Find("NPCSpawner");
            NPCSpawner run = manager.GetComponent<NPCSpawner>();
            run.SetPlayerIndex(index);
        }
    }
}