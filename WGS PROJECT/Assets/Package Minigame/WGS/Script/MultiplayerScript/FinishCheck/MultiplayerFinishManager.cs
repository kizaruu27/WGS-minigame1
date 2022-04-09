
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class MultiplayerFinishManager : MonoBehaviour
{

    public static MultiplayerFinishManager instance;
    [SerializeField] GameObject finishUI;

    List<LeaderboardManager.CLeaderboardItem> leaderboard;

    public bool isFinish { get; private set; }

    public void SetLeaderboardData(List<LeaderboardManager.CLeaderboardItem> leaderboardItems)
    {
        leaderboard = leaderboardItems;
    }

    public List<LeaderboardManager.CLeaderboardItem> GetLeaderboardData() => leaderboard;


    public void Finish(bool isCrossFinishLine)
    {
        isFinish = isCrossFinishLine;
        finishUI.SetActive(isCrossFinishLine);
    }

    public void OnClickBackToMenu()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(2);
    }
}
