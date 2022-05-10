using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UsernameInput : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TextMeshProUGUI username;

    public void SetUsername()
    {
        username.text = usernameField.text;
        PlayerPrefs.SetString("Username", username.text);
    }
}
