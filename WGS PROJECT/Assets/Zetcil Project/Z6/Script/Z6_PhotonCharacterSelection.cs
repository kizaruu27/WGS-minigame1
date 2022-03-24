using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Z6_PhotonCharacterSelection : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerSelect(string aPlayer)
    {
        PlayerPrefs.SetString("CurrentPlayer", aPlayer);
    }
}
