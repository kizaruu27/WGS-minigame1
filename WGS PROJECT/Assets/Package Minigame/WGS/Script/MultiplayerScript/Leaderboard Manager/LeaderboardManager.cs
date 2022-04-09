using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;
    PhotonView PV;

    [System.Serializable]
    public class CLeaderboardItem
    {
        public string PlayerName;
        public float PlayerScore;
    }

    private void Awake()
    {
        instance = this;
        PV = GetComponent<PhotonView>();
    }

    static int SortAsc(CLeaderboardItem p1, CLeaderboardItem p2)
    {
        return p1.PlayerScore.CompareTo(p2.PlayerScore);
    }

    static int SortDesc(CLeaderboardItem p1, CLeaderboardItem p2)
    {
        return p2.PlayerScore.CompareTo(p1.PlayerScore);
    }

    [Header("Leaderboard Item")]
    public List<Text> LeaderboardText;

    [Header("Leaderboard Button")]
    public List<Button> LeaderboardListButton;

    [Header("Leaderboard Item")]
    public List<CLeaderboardItem> LeaderboardItem;

    public void InitializePlayer(string aPlayerName, float aScore)
    {
        CLeaderboardItem temp = new CLeaderboardItem();
        temp.PlayerName = aPlayerName;
        temp.PlayerScore = aScore;
        LeaderboardItem.Add(temp);
    }

    public void SetButtonVisible(int aPlayerIndex, bool aStatus)
    {
        if (aPlayerIndex < LeaderboardText.Count)
        {
            LeaderboardText[aPlayerIndex].transform.parent.gameObject.SetActive(aStatus);
        }
    }

    //! buat update playername pertama kali sesuai dengan index
    public void UpdatePlayerName(int aPlayerIndex, string aPlayerName)
    {
        if (aPlayerIndex < LeaderboardItem.Count)
        {
            LeaderboardItem[aPlayerIndex].PlayerName = aPlayerName;
        }
    }

    //! buat update playerscore setiap kali terjadi penambahan score
    public void UpdatePlayerScore(string aPlayerName, float aScore)
    {
        for (int i = 0; i < LeaderboardItem.Count; i++)
        {
            if (LeaderboardItem[i].PlayerName == aPlayerName)
            {

                LeaderboardItem[i].PlayerScore += aScore;
                // PV.RPC("OrderPlayerScore", RpcTarget.AllBufferedViaServer, i, aScore);
            }
        }
    }

    void ItemFocusToPlayer()
    {
        List<Button> listButton = LeaderboardListButton.Select(val =>
        {
            val.GetComponent<Image>().color = Color.white;
            val.GetComponentInChildren<Text>().color = new Color32(50, 50, 50, 255);

            return val;

        }).ToList();

        int playerIndex = LeaderboardItem.FindIndex(val => val.PlayerName == PhotonNetwork.LocalPlayer.NickName);

        Button playerItem = listButton[playerIndex];
        playerItem.GetComponent<Image>().color = Color.red;
        playerItem.GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255);
    }


    public void UpdateLeaderboard()
    {
        // PV.RPC("SortPosisition", RpcTarget.AllBuffered);
        LeaderboardItem.Sort(SortDesc);
        for (int i = 0; i < LeaderboardText.Count; i++)
        {
            LeaderboardText[i].text = LeaderboardItem[i].PlayerName;
        }
    }

    public void GetLeaderboardData()
    {
        GameObject ds = GameObject.FindGameObjectWithTag("Finish UI");
        ds.GetComponent<MultiplayerFinishManager>().SetLeaderboardData(LeaderboardItem);
    }

    void LateUpdate()
    {
        ItemFocusToPlayer();
        UpdateLeaderboard();
        GetLeaderboardData();
    }



    public void TestUpdatePlayer1()
    {
        UpdatePlayerScore("Player 1", 100);
    }

    public void TestUpdatePlayer2()
    {
        UpdatePlayerScore("Player 2", 100);
    }

    public void TestUpdatePlayer3()
    {
        UpdatePlayerScore("Player 3", 100);
    }

    public void TestVisibleButton8()
    {
        SetButtonVisible(7, false);
    }
}