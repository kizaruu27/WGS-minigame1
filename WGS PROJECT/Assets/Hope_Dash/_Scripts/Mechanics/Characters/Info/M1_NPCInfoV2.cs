using UnityEngine;
using Photon.Pun;
using RunMinigames.Mechanics.Checkpoint;
using RunMinigames.Manager.Leaderboard;

namespace RunMinigames.Mechanics.Characters
{

    public class M1_NPCInfoV2 : M1_CharactersInfo
    {
        public static M1_NPCInfoV2 info;

        private new void Awake()
        {
            base.Awake();
            info = this;
        }

        private void Start()
        {
            if (type.IsMultiplayer)
            {
                view.RPC(nameof(UpdateCharacterName), RpcTarget.AllBuffered, CharaID, CharaName);
            }
            else
            {
                CharaName = gameObject.name;
                M1_GameplayLeaderboardManager.instance.UpdatePlayerName(CharaID, CharaName);
            }

            CharaViewName.text = CharaName;
        }


        protected override void CheckTypeUpdatePodium(M1_GameCheckpoint checkpoint)
        {
            if (type.IsMultiplayer)
            {
                view.RPC(
                    nameof(UpdatePodiumList), RpcTarget.AllBuffered, //RPC Arguments
                     CharaID, timer, CharaName //Method Arguments
                    );
            }
            else
            {
                GameObject finishUI = GameObject.FindGameObjectWithTag("Finish UI");
                finishUI.GetComponent<M1_FinishLeaderboard>()
                    .Finish(CharaID, timer, CharaName);
            }
        }

        public void SetNPCInfo(int newID, string newName)
        {
            if (type.IsMultiplayer)
                view.RPC(nameof(SetNameNPC), RpcTarget.AllBuffered, newID, newName);
        }

        [PunRPC]
        void SetNameNPC(int newID, string newName)
        {
            CharaID = newID;
            CharaName = newName;
        }
    }
}