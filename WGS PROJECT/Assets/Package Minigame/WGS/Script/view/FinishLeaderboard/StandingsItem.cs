
using UnityEngine;
using UnityEngine.UI;

public class StandingsItem : MonoBehaviour
{
    [Header("Text Component")]
    public Text Rank;
    public Text Name;
    public Text Score;

    public void SetColorItem(bool IsMine = false)
    {
        if (IsMine)
        {
            // Image colorItem = gameObject.GetComponent<Image>();
            // colorItem.color = Color.green;

            Text[] colorTextItem = gameObject.GetComponentsInChildren<Text>();

            foreach (var item in colorTextItem)
            {
                item.color = Color.black;
            }
        }
    }

    public void SetHighlightPlayerDC()
    {
        Image colorItem = gameObject.GetComponent<Image>();
        // colorItem.color = Color.red;

        Text[] colorTextItem = gameObject.GetComponentsInChildren<Text>();

        foreach (var item in colorTextItem)
        {
            item.color = Color.white;
        }
    }
}