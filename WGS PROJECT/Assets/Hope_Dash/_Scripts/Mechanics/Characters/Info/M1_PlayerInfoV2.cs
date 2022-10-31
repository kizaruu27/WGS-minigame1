using UnityEngine;
using Photon.Pun;
using RunMinigames.Mechanics.Checkpoint;
using RunMinigames.Manager.Leaderboard;
using M1_SimpleJSON;

namespace RunMinigames.Mechanics.Characters
{
    public class M1_PlayerInfoV2 : M1_CharactersInfo
    {
        private new void Awake()
        {
            base.Awake();

            if (type.IsMultiplayer && view.IsMine)
            {
                GameObject
                    .FindGameObjectWithTag("Camera")
                    .GetComponent<RunMinigames.Mechanics.Cam.M1_CameraController>().Player = transform;
            }

            // CharaName = type.IsMultiplayer ?
            //     view.Owner.NickName :
            //     PhotonNetwork.NickName ?? JSON.Parse(PlayerPrefs.GetString("LocalPlayerData"))["uname"];

            // CharaName = PhotonNetwork.NickName;
            CharaName = PhotonNetwork.LocalPlayer.NickName;

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

            M1_GameplayLeaderboardManager.instance.UpdatePlayerName(CharaID, CharaName);
        }

        protected override void CheckTypeUpdatePodium(M1_GameCheckpoint checkpoint)
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
                finishUI.GetComponent<M1_FinishLeaderboard>()
                    .Finish(isPlayerFinish, CharaID, timer, CharaName);
            }
        }

        [PunRPC]
        void SetPlayerName(string name) => M1_GameplayLeaderboardManager.instance.SetPlayerName(name);


        [PunRPC]
        void SetAvatarIndex(int index)
        {
            GameObject manager = GameObject.Find("NPCSpawner");
            M1_NPCSpawner run = manager.GetComponent<M1_NPCSpawner>();
            run.SetPlayerIndex(index);
        }
    }
}