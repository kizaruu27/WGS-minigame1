using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PodiumUIHandler : MonoBehaviour
{
    RectTransform rectTransform;
    [SerializeField] TextMeshProUGUI playerPositionTxt;
    [SerializeField] TextMeshProUGUI playerWinTxt;
    [SerializeField] TextMeshProUGUI playerPointsTxt;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (FinishChecker.finishChecker.isFinish)
        {
            rectTransform.anchoredPosition = new Vector2 (960f, -573);
            rectTransform.sizeDelta = new Vector2 (930.54f, 400f);

            if (WinnerChecker.win.isWin)
            {
                playerWinTxt.gameObject.SetActive(true);
                playerPointsTxt.gameObject.SetActive(true);

            }
            else
            {
                playerPositionTxt.gameObject.SetActive(true);
                playerPointsTxt.gameObject.SetActive(true);

            }
        }
    }
}
