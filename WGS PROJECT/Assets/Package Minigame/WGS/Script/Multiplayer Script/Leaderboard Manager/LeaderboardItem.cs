using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class LeaderboardItem : MonoBehaviour
{

    public TextMeshProUGUI playerRankText;
    public TextMeshProUGUI playerNameText;

    public void SetPlayerName(Player player) => playerNameText.text = player.NickName;
    public void SetPlayerName(string newName) => playerNameText.text = newName;
    public void SetPlayerRank(string newRankPosition) {
        
        for (int i = 0; i < PhotonNetwork.CountOfPlayers; i++){
            playerRankText.text = newRankPosition;
        }
        
    } 

    
}
