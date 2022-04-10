
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

    [Header("List Player")]
    List<PlayerFinishModel> playerFinishList = new List<PlayerFinishModel>();

    PhotonView view;


    private void Start()
    {
        view = GetComponent<PhotonView>();
    }


    public void InitializePlayer(int id, string name, float time)
    {
        PlayerFinishModel playerFinish = new PlayerFinishModel();
        playerFinish.name = name;
        playerFinish.time = time;
        playerFinish.id = id;

        playerFinishList.Add(playerFinish);
    }

    public IEnumerable<PlayerFinishModel> GetLeaderboardData() => playerFinishList.OrderBy(val => val.time);

    // PLAYER
    public void Finish(bool isPlayerCrossFinish, int id, float time, string name)
    {
        if (PhotonNetwork.LocalPlayer.NickName == name)
        {
            LeaderboardUI.SetActive(!isPlayerCrossFinish);
            finishUI.SetActive(isPlayerCrossFinish);
        }

        InitializePlayer(id, name, time);
    }

    // NPC
    public void Finish(int id, float time, string name)
    {
        Debug.Log(name + " is duplicate " + playerFinishList.Any(item => item.id == id));
        if (!playerFinishList.Any(item => item.id == id))
        {
            InitializePlayer(id, name, time);
        }
    }

    public void OnClickBackToMenu()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(2);
    }
}
