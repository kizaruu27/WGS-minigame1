

using UnityEngine;
using Photon.Pun;
using System.Linq;
using System.Collections.Generic;

public class MultiplayerFinishUI : MonoBehaviour
{
    public static MultiplayerFinishUI instance;

    [Header("Components")]
    public MultiplayerFinishManager finishManager;
    public RowUI row;

    [Header("Player Score")]
    int score = 1000;


    [Header("Player List")]

    IEnumerable<PlayerFinishModel> PlayerFinish;
    List<RowUI> cachePlayerList = new List<RowUI>();
    private void Awake() => instance = this;

    private void Update()
    {
        RemovePlayerListCache();
        ShowPlayerList();
        WaitingPlayerToFinish();
        PlayerDiscHighlight();

        if (cachePlayerList.Count == (int)PhotonNetwork.CurrentRoom.MaxPlayers) return;
    }

    public void ShowPlayerList()
    {
        PlayerFinish = finishManager.GetLeaderboardData();

        foreach (var item in PlayerFinish.Select((value, index) => new { index, value }))
        {
            RowUI rowData = Instantiate(row, transform).GetComponent<RowUI>();

            rowData.SetColorItem(item.value.id == PhotonNetwork.LocalPlayer.ActorNumber - 1);

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
                RowUI rowData = Instantiate(row, transform).GetComponent<RowUI>();

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
                RowUI rowData = Instantiate(row, transform).GetComponent<RowUI>();

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


    public void OnClickBackToMenu()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(2);
    }


}