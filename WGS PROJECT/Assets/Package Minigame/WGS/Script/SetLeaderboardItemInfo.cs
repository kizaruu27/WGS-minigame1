using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetLeaderboardItemInfo : MonoBehaviour
{

    public TextMeshProUGUI positionText;
    public TextMeshProUGUI playerNameText;
    void Start()
    {
        
    }

    public void SetPositionText(string newPosition)
    {
        positionText.text = newPosition;
    }

    public void SetPlayerNameText(string newPlayerName)
    {
        playerNameText.text = newPlayerName;
    }

}
