using UnityEngine;
using TMPro;
using Photon.Realtime;

public class LeaderboardItem : MonoBehaviour
{

    public TextMeshProUGUI playerRankText;
    public TextMeshProUGUI playerNameText;

    public void Initialize(Player _player) => playerNameText.text = _player.NickName;  

    
}
