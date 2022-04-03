using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Z7_Leaderboard : MonoBehaviour
{

    [System.Serializable]
    public class CLeaderboardItem
    {
        public string PlayerName;
        public float PlayerScore;
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

    //buat update playername pertama kali sesuai dengan index
    public void UpdatePlayerName(int aPlayerIndex, string aPlayerName)
    {
        if (aPlayerIndex < LeaderboardItem.Count)
        {
            LeaderboardItem[aPlayerIndex].PlayerName = aPlayerName;
        }
    }

    //buat update playerscore setiap kali terjadi penambahan score
    public void UpdatePlayerScore(string aPlayerName, float aScore)
    {
        for (int i = 0; i < LeaderboardItem.Count; i++)
        {
            if (LeaderboardItem[i].PlayerName == aPlayerName)
            {
                LeaderboardItem[i].PlayerScore += aScore;
            }
        }
    }
    //buat UI player leaderboard
    public void UpdateLeaderboard()
    {
        LeaderboardItem.Sort(SortDesc);
        for(int i=0; i< LeaderboardText.Count; i++)
        {
            LeaderboardText[i].text = LeaderboardItem[i].PlayerName;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdatePlayerName(1, "taufiq");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateLeaderboard();
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
