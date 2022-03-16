using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinnerChecker : MonoBehaviour
{
    public static WinnerChecker win;
    LapCounter lap;

    int playerPoints;

    public bool isWin;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI playerPositionTxt;
    [SerializeField] TextMeshProUGUI playerWinTxt;
    [SerializeField] TextMeshProUGUI playerPointsTxt;
    

    
    private void Awake()
    {
        win = this;
    }

    void Start()
    {
        lap = GetComponent<LapCounter>();
        isWin = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (FinishChecker.finishChecker.isFinish)
        {
            switch (lap.playerPosition)
            {
                case 1:
                isWin = true;
                playerPoints = 1000;
                playerPointsTxt.text = "You got " + playerPoints + " points";
                break;

                case 2:
                playerPoints = 900;
                playerPositionTxt.text = "You're at " + lap.playerPosition.ToString() + "nd Position";
                playerPointsTxt.text = "You got " + playerPoints + " points";
                break;

                case 3:
                playerPoints = 800;
                playerPositionTxt.text = "You're at " + lap.playerPosition.ToString() + "rd Position";
                playerPointsTxt.text = "You got " + playerPoints + " points";
                break;

                case 4:
                playerPoints = 700;
                playerPositionTxt.text = "You're at " + lap.playerPosition.ToString() + "th Position";
                playerPointsTxt.text = "You got " + playerPoints + " points";
                break;

                case 5:
                playerPoints = 600;
                playerPositionTxt.text = "You're at " + lap.playerPosition.ToString() + "th Position";
                playerPointsTxt.text = "You got " + playerPoints + " points";
                break;

                case 6:
                playerPoints = 500;
                playerPositionTxt.text = "You're at " + lap.playerPosition.ToString() + "th Position";
                playerPointsTxt.text = "You got " + playerPoints + " points";
                break;

                case 7:
                playerPoints = 400;
                playerPositionTxt.text = "You're at " + lap.playerPosition.ToString() + "th Position";
                playerPointsTxt.text = "You got " + playerPoints + " points";
                break;

                case 8:
                playerPoints = 300;
                playerPositionTxt.text = "You're at " + lap.playerPosition.ToString() + "th Position";
                playerPointsTxt.text = "You got " + playerPoints + " points";
                break;
            }
        }
    }
}
