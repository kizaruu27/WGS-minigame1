
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RunMinigames.Models;
using Photon.Pun;


namespace RunMinigames.Manager.Leaderboard
{
    public class FinishLeaderboard : MonoBehaviour
    {
        public static FinishLeaderboard instance;

        [Header("Canvas UI")]
        [SerializeField] GameObject finishUI;
        [SerializeField] GameObject LeaderboardUI;

        [Header("Player List")]
        List<MPlayerFinish> playerFinishList = new List<MPlayerFinish>();

        PhotonView pv;

        private void Awake() => pv = GetComponent<PhotonView>();
        private void Update()
        {
            pv.RPC("SendCount", RpcTarget.AllBuffered, playerFinishList.Count);
        }
        public int TotalPlayersDisconnect { get; set; }

        public void InitializePlayer(int id, string name, float time)
        {
            MPlayerFinish playerFinish = new MPlayerFinish();
            playerFinish.name = name;
            playerFinish.time = time;
            playerFinish.id = id;

            playerFinishList.Add(playerFinish);
        }

        public IEnumerable<MPlayerFinish> GetLeaderboardData() => playerFinishList.OrderBy(player => player.time).ThenBy(player => player.name);

        // PLAYER
        public void Finish(bool isPlayerCrossFinish, int id, float time, string name)
        {
            finishUI.SetActive(isPlayerCrossFinish);
            LeaderboardUI.SetActive(!isPlayerCrossFinish);


            if (PhotonNetwork.LocalPlayer.ActorNumber - 1 == id)
            {
                for (int i = 0; i < LeaderboardUI.transform.childCount; i++)
                {
                    // Debug.Log($"child number: {i}");
                    GameObject child = LeaderboardUI.transform.GetChild(i).gameObject;
                    child.gameObject.SetActive(!isPlayerCrossFinish);
                }
            }

            if (!playerFinishList.Any(item => item.id == id)) InitializePlayer(id, name, time);
        }

        // NPC
        public void Finish(int id, float time, string name)
        {
            if (!playerFinishList.Any(item => item.id == id)) InitializePlayer(id, name, time);
        }

        // send total player finish to leaderboard manager
        [PunRPC]
        void SendCount(int total) => GameplayLeaderboardManager.instance.ShowPlayerRank(total);
    }

}
