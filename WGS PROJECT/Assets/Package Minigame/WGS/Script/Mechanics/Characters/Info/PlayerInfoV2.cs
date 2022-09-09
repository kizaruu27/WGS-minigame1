using UnityEngine;
using Photon.Pun;
using RunMinigames.Mechanics.Checkpoint;
using RunMinigames.Manager.Leaderboard;
using SimpleJSON;

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

            // CharaName = type.IsMultiplayer ?
            //     view.Owner.NickName :
            //     PhotonNetwork.NickName ?? JSON.Parse(PlayerPrefs.GetString("LocalPlayerData"))["uname"];

            CharaName = PhotonNetwork.NickName;

            // CharaID = type.IsMultiplayer ? view.Owner.ActorNumber - 1 : 0;

            int playerIndex = PlayerPrefs.GetInt("positionIndex"); //!-> Index from player list in the room
            CharaID = type.IsMultiplayer ? playerIndex : 0;

            if (type.IsMultiplayer)
            {
                view.RPC(nameof(SetAvatarIndex), RpcTarget.AllBuffered, PlayerPrefs.GetInt("playerAvatar"));
            }

            CharaViewName.text = CharaName;
        }

        private void Start()
        {
            if (type.IsMultiplayer)
            {
                view.RPC(nameof(UpdateCharacterName), RpcTarget.AllBuffered, CharaID, CharaName);
                view.RPC(nameof(SetPlayerName), RpcTarget.AllBuffered, CharaName);
            }

            GameplayLeaderboardManager.instance.UpdatePlayerName(CharaID, CharaName);
        }

        protected override void CheckTypeUpdatePodium(GameCheckpoint checkpoint)
        {
            if (type.IsMultiplayer)
            {
                view.RPC(
                    nameof(UpdatePodiumList), RpcTarget.AllBuffered, //RPC Arguments
                    isPlayerFinish, CharaID, timer, CharaName //Method Arguments
                    );
            }
            else
            {
                GameObject finishUI = GameObject.FindGameObjectWithTag("Finish UI");
                finishUI.GetComponent<FinishLeaderboard>()
                    .Finish(isPlayerFinish, CharaID, timer, CharaName);
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