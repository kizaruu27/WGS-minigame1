using UnityEngine;
using Photon.Pun;
using RunMinigames.Mechanics.Checkpoint;
using RunMinigames.Manager.Leaderboard;

namespace RunMinigames.Mechanics.Characters
{

    public class NPCInfoV2 : CharactersInfo
    {
        public static NPCInfoV2 info;

        private new void Awake()
        {
            base.Awake();
            info = this;
        }

        private void Start()
        {
            if (type.IsMultiplayer)
            {
                view.RPC("UpdateCharacterName", RpcTarget.AllBuffered, CharaID, CharaName);
            }
            else
            {
                CharaName = gameObject.name;
                GameplayLeaderboardManager.instance.UpdatePlayerName(CharaID, CharaName);
            }

            // CharaViewName.text = CharaName;
        }


        protected override void CheckTypeUpdatePodium(GameCheckpoint checkpoint)
        {
            if (type.IsMultiplayer)
            {
                view.RPC(
                    "UpdatePodiumList", RpcTarget.AllBuffered, //RPC Arguments
                     CharaID, timer, CharaName //Method Arguments
                    );
            }
            else
            {
                GameObject finishUI = GameObject.FindGameObjectWithTag("Finish UI");
                finishUI.GetComponent<FinishLeaderboard>()
                    .Finish(CharaID, timer, CharaName);
            }
        }

        public void SetNPCInfo(int newID, string newName)
        {
            if (type.IsMultiplayer)
                view.RPC("SetNameNPC", RpcTarget.AllBuffered, newID, newName);
        }

        [PunRPC]
        void SetNameNPC(int newID, string newName)
        {
            CharaID = newID;
            CharaName = newName;
        }
    }
}