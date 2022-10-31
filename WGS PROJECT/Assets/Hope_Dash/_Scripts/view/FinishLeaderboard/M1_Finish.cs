using UnityEngine;
using Photon.Pun;
using System.Linq;
using System.Collections.Generic;
using RunMinigames.Manager.Leaderboard;
using RunMinigames.Manager.Game;
using RunMinigames.Models;
using UnityEngine.SceneManagement;


public class M1_Finish : MonoBehaviourPunCallbacks
{
    public static M1_Finish instance;

    [Header("Components")]
    public M1_FinishLeaderboard m1FinishManager;
    public M1_StandingsItem row;

    [Header("Player List")]

    IEnumerable<M1_MPlayerFinish> PlayerFinish;
    List<M1_StandingsItem> cachePlayerList = new List<M1_StandingsItem>();


    M1_GameManager type;
    private void Awake()
    {
        instance = this;
        type = GameObject.Find("GameManager").GetComponent<M1_GameManager>();
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
        PlayerFinish = m1FinishManager.GetLeaderboardData();

        foreach (var item in PlayerFinish.Select((value, index) => new { index, value }))
        {
            M1_StandingsItem rowData = Instantiate(row, transform).GetComponent<M1_StandingsItem>();

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
            for (int i = 0; i < ((int)PhotonNetwork.CurrentRoom.MaxPlayers - m1FinishManager.TotalPlayersDisconnect) - cachePlayerList.Count; i++)
            {
                M1_StandingsItem rowData = Instantiate(row, transform).GetComponent<M1_StandingsItem>();

                rowData.Rank.text = "";
                rowData.Name.text = "Waiting Other Player";
                rowData.Score.text = "";

                cachePlayerList.Add(rowData);
            }
        }
    }

    public void PlayerDiscHighlight()
    {
        int PDC = m1FinishManager.TotalPlayersDisconnect;
        if (PDC > 0)
        {
            for (int i = 0; i < PDC; i++)
            {
                M1_StandingsItem rowData = Instantiate(row, transform).GetComponent<M1_StandingsItem>();

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

        string minuteTxt = minute >= 1 ? $"{minute} M " : "";
        string secondsTxt = seconds != 0 ? $"{seconds} S " : "";

        return minuteTxt + secondsTxt;
    }

    public void OnClickExitRoom()
    {
        if (M1_GameManager.instance.IsMultiplayer)
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