using UnityEngine;
using TMPro;
using Photon.Pun;

public class LeaderboardItem : MonoBehaviour
{

    public TextMeshProUGUI playerRankText;
    public TextMeshProUGUI playerNameText;

    public void SetPlayerName(string newName) => playerNameText.text = newName;
    public void SetPositionText(string newPosition) => playerRankText.text = newPosition;
}
