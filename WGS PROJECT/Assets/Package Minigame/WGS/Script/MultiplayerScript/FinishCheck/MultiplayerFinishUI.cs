

using UnityEngine;
using Photon.Pun;
using System.Linq;

public class MultiplayerFinishUI : MonoBehaviour
{
    public MultiplayerFinishManager finishManager;
    public RowUI row;
    public GameObject leaderboardManager;

    int score = 1000;


    private void Start()
    {
        var PlayerFinish = finishManager.GetLeaderboardData();

        foreach (var item in PlayerFinish.Select((value, index) => new { index, value }))
        {
            RowUI rowData = Instantiate(row, transform).GetComponent<RowUI>();

            rowData.SetColorItem(item.value.PlayerName == PhotonNetwork.LocalPlayer.NickName);

            rowData.Rank.text = GenerateRankText(item.index);
            rowData.Name.text = item.value.PlayerName;
            rowData.Score.text = $"{score - (item.index * 100)}";
        }

        leaderboardManager.SetActive(false);
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


}