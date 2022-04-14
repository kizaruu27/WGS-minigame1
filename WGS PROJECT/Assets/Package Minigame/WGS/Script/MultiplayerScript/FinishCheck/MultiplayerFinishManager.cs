
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Photon.Pun;

public class MultiplayerFinishManager : MonoBehaviour
{
    public static MultiplayerFinishManager instance;

    [Header("Canvas UI")]
    [SerializeField] GameObject finishUI;
    [SerializeField] GameObject LeaderboardUI;

    [Header("Player List")]
    List<PlayerFinishModel> playerFinishList = new List<PlayerFinishModel>();

    public void InitializePlayer(int id, string name, float time)
    {
        PlayerFinishModel playerFinish = new PlayerFinishModel();
        playerFinish.name = name;
        playerFinish.time = time;
        playerFinish.id = id;

        playerFinishList.Add(playerFinish);
    }

    public IEnumerable<PlayerFinishModel> GetLeaderboardData() => playerFinishList.OrderBy(player => player.time);

    // PLAYER
    public void Finish(bool isPlayerCrossFinish, int id, float time, string name)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber - 1 == id)
        {
            LeaderboardUI.SetActive(!isPlayerCrossFinish);
            finishUI.SetActive(isPlayerCrossFinish);
        }

        if (!playerFinishList.Any(item => item.id == id)) InitializePlayer(id, name, time);
    }

    // NPC
    public void Finish(int id, float time, string name)
    {
        if (!playerFinishList.Any(item => item.id == id)) InitializePlayer(id, name, time);
    }
}
