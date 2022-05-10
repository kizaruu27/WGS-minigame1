using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadUsername : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    // Start is called before the first frame update
    void Start()
    {
        playerName.text = PlayerPrefs.GetString("Username");
    }
}
