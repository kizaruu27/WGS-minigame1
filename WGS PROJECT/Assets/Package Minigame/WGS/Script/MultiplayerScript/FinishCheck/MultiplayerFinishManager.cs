
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;

public class MultiplayerFinishManager : MonoBehaviour
{

    public static MultiplayerFinishManager instance;
    [SerializeField] GameObject finishUI;


    [SerializeField] TextMeshProUGUI GreetingText;
    [SerializeField] TextMeshProUGUI ScoreText;

    PhotonView view;
    List<LeaderboardManager.CLeaderboardItem> leaderboard;

    int score = 1000;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }


    public void SetLeaderboardData(List<LeaderboardManager.CLeaderboardItem> leaderboardItems)
    {
        leaderboard = leaderboardItems;
    }


    string GenerateGreetingText(int rank)
    {
        int indexPosition = rank + 1;

        string messageByPosition = indexPosition switch
        {
            2 => "2nd",
            3 => "3rd",
            _ => $"{indexPosition}th"
        };

        return indexPosition > 1 ? $"Congratulation You Win! in Position {messageByPosition} place" : "Congratulation You're the Winner";
    }


    public void Finish()
    {
        int PlayerRank = leaderboard.FindIndex(val => val.PlayerName == PhotonNetwork.LocalPlayer.NickName);

        GreetingText.text = GenerateGreetingText(PlayerRank);
        ScoreText.text = $"You Got {score - (PlayerRank * 100)} Points! Yeayy!!!";

        finishUI.SetActive(true);
    }

    public void OnClickBackToMenu()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(2);
    }
}
