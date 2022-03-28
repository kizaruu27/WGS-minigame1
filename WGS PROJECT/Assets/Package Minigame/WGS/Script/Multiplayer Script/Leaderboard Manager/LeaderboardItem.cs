using UnityEngine;
using TMPro;
using Photon.Realtime;

public class LeaderboardItem : MonoBehaviour
{

    public TextMeshProUGUI playerRankText;
    public TextMeshProUGUI playerNameText;

    public void SetPlayerName(string newName) => playerNameText.text = newName;
    public void SetPlayerName(Player newPlayer) => playerNameText.text = newPlayer.NickName;
    public void SetPositionText(string newPosition) => playerRankText.text = newPosition;

    
}
