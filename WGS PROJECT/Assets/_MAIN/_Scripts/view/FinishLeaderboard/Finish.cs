using UnityEngine;
using Photon.Pun;
using System.Linq;
using System.Collections.Generic;
using RunMinigames.Manager.Leaderboard;
using RunMinigames.Manager.Game;
using RunMinigames.Models;
using UnityEngine.SceneManagement;


public class Finish : MonoBehaviourPunCallbacks
{
    public static Finish instance;

    [Header("Components")]
    public FinishLeaderboard finishManager;
    public StandingsItem row;

    [Header("Player List")]

    IEnumerable<MPlayerFinish> PlayerFinish;
    List<StandingsItem> cachePlayerList = new List<StandingsItem>();


    GameManager type;
    private void Awake()
    {
        instance = this;
        type = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        RemovePlayerListCache();
        ShowPlayerList();

        if (type.IsMultiplayer)
        {
            WaitingPlayerToFinish();
            PlayerDiscHighlight();

            if (cachePlayerList.Count == (int)PhotonNetwork.CurrentRoom.MaxPlayers) return;
        }


    }

    public void ShowPlayerList()
    {
        PlayerFinish = finishManager.GetLeaderboardData();

        foreach (var item in PlayerFinish.Select((value, index) => new { index, value }))
        {
            StandingsItem rowData = Instantiate(row, transform).GetComponent<StandingsItem>();

            rowData.SetColorItem(item.value.id == PhotonNetwork.LocalPlayer.ActorNumber - 1 || item.value.id == 0);

            rowData.Rank.text = GenerateRankText(item.index);
            rowData.Name.text = item.value.name.Length <= 20 ? item.value.name : item.value.name.Substring(0, 20) + "...";
            rowData.Score.text = GenerateTimeText(item.value.time);

            cachePlayerList.Add(rowData);
        }
    }

    public void WaitingPlayerToFinish()
    {
        if (PhotonNetwork.CurrentRoom.MaxPlayers > cachePlayerList.Count)
        {
            for (int i = 0; i < ((int)PhotonNetwork.CurrentRoom.MaxPlayers - finishManager.TotalPlayersDisconnect) - cachePlayerList.Count; i++)
            {
                StandingsItem rowData = Instantiate(row, transform).GetComponent<StandingsItem>();

                rowData.Rank.text = "";
                rowData.Name.text = "Waiting Other Player";
                rowData.Score.text = "";

                cachePlayerList.Add(rowData);
            }
        }
    }

    public void PlayerDiscHighlight()
    {
        int PDC = finishManager.TotalPlayersDisconnect;
        if (PDC > 0)
        {
            for (int i = 0; i < PDC; i++)
            {
                StandingsItem rowData = Instantiate(row, transform).GetComponent<StandingsItem>();

                rowData.SetHighlightPlayerDC();

                rowData.Rank.text = "";
                rowData.Name.text = "Disconnected";
                rowData.Score.text = "";

                cachePlayerList.Add(rowData);
            }
        }

    }


    public void RemovePlayerListCache()
    {
        foreach (var item in cachePlayerList) Destroy(item.gameObject);

        cachePlayerList.Clear();
    }

    string GenerateRankText(int rank)
    {
        int indexPosition = rank + 1;

        return indexPosition switch
        {
            1 => "1st",
            2 => "2nd",
            3 => "3rd",
            _ => $"{indexPosition}th"
        };
    }

    string GenerateTimeText(float time)
    {
        int totalSecondsInMinute = 60;

        float minuteInFloat = time / totalSecondsInMinute;
        int minute = minuteInFloat >= 1 ? (int)minuteInFloat : 0;

        int seconds = minute != 0 ? (int)time - (minute * totalSecondsInMinute) : (int)time;

        string minuteTxt = minute >= 1 ? $"{minute} Menit " : "";
        string secondsTxt = seconds != 0 ? $"{seconds} Detik " : "";

        return minuteTxt + secondsTxt;
    }

    public void OnClickExitRoom()
    {
        if (GameManager.instance.IsMultiplayer)
        {
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene("WGS2_Lobby");
        }
    }

    public override void OnLeftRoom()
    {
        Debug.LogError("Left room");
        PhotonNetwork.LoadLevel("WGS2_Lobby");
    }
}