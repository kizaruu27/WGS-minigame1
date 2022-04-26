using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LeaderboardManager : MonoBehaviourPunCallbacks
{
    public static LeaderboardManager instance;
    PhotonView PV;

    [System.Serializable]
    public class CLeaderboardItem
    {
        public string PlayerName;
        public float PlayerScore;

        public int PlayerID;
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
    public List<string> TotalCachedPlayers = new List<string>();

    [Header("Player Disconnect info")]
    [SerializeField] int playerDisconnected = 0;

    [Header("Animation Podium")]
    public List<GameObject> UIPositionObj;

    public void ShowPlayerRank(int playerFinish) 
    {
        // Debug.Log(UIPositionObj[1].activeInHierarchy == false);
        for (int i = 0; i < playerFinish; i++){
            if(UIPositionObj[i].activeInHierarchy == false){
                UIPositionObj[i].SetActive(true);
                UIPositionObj[i].GetComponent<Animation>().Play("showRank");
            }
        }
    }

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
            LeaderboardItem[aPlayerIndex].PlayerID = aPlayerIndex;
        }
    }

    public void SetPlayerName(string PlayerName)
    {
        TotalCachedPlayers.Add(PlayerName);
    }

    //! buat update playerscore setiap kali terjadi penambahan score
    public void UpdatePlayerScore(string aPlayerName, float aScore)
    {
        for (int i = 0; i < LeaderboardItem.Count; i++)
        {
            if (LeaderboardItem[i].PlayerName == aPlayerName)
            {
                LeaderboardItem[i].PlayerScore = aScore; // change the increment
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

        int playerIndex = LeaderboardItem.FindIndex(val => val.PlayerID == PhotonNetwork.LocalPlayer.ActorNumber - 1);

        Button playerItem = listButton[playerIndex];
        playerItem.GetComponent<Image>().color = Color.green;
        playerItem.GetComponentInChildren<Text>().color = Color.black;

        if (playerDisconnected > 0)
        {
            for (int i = 1; i <= playerDisconnected; i++)
            {
                Button highlight = listButton[LeaderboardListButton.Count - i];
                highlight.GetComponent<Image>().color = Color.red;
                highlight.GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255);
            }
        }
    }


    public void UpdateLeaderboard()
    {
        int TotalPlayerDisconnect = LeaderboardText.Count - LeaderboardItem.Count;

        LeaderboardItem.Sort(SortDesc);

        foreach (Text EmptyPlayer in LeaderboardText) EmptyPlayer.text = "Player Disconnected";

        for (int i = 0; i < LeaderboardText.Count - TotalPlayerDisconnect; i++)
        {
            LeaderboardText[i].text = LeaderboardItem[i].PlayerName;
        }

        PV.RPC("sentPlayerDiscCount", RpcTarget.AllBuffered, playerDisconnected);
    }

    public void RemovePlayerOnDisconnect()
    {
        List<string> TotalNetworkPlayers = PhotonNetwork.PlayerList.Select(Player => Player.NickName).ToList();
        List<string> DisconnectPlayers = TotalCachedPlayers.Except(TotalNetworkPlayers).ToList();

        var TotalPlayersLeft = LeaderboardItem.Where(Player => !DisconnectPlayers.Contains(Player.PlayerName)).ToList();

        LeaderboardItem.Clear();
        LeaderboardItem = TotalPlayersLeft;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        playerDisconnected++;


        // Debug.Log($"ID Player disc: {otherPlayer.ActorNumber}");
    }


    void LateUpdate()
    {

        ItemFocusToPlayer();
        RemovePlayerOnDisconnect();
        UpdateLeaderboard();


    }


    [PunRPC]
    void sentPlayerDiscCount(int total)
    {
        MultiplayerFinishManager FinishManager = GameObject.FindObjectOfType<MultiplayerFinishManager>();
        FinishManager.TotalPlayersDisconnect = total;
    }
}
