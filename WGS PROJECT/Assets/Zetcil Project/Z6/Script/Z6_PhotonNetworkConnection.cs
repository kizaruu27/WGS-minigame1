using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Z6_PhotonNetworkConnection : MonoBehaviourPunCallbacks
{
    public Text ConnectStatus;

    // Start is called before the first frame update
    void Start()
    {
        //Photon #1
        ConnectStatus.text = "Connecting to server...";
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        //Photon #2
        ConnectStatus.text = "Connected: " + PhotonNetwork.CloudRegion;
    }
}
