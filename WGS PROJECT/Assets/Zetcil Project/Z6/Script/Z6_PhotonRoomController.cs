using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Z6_PhotonRoomController : MonoBehaviourPunCallbacks
{
    [Header("Network Scene Index")]
    public int PhotonGameSceneIndex;

    [Header("Room Status")]
    public UnityEvent JoinRoomSuccessEvent;
    public UnityEvent JoinRoomFailedEvent;


    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        //Photon #7
        Debug.Log("Room Controller: Joined Room");
        JoinRoomSuccessEvent.Invoke();
        StartGame();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //Photon #8
        Debug.Log("Room Controller: " + message);
        JoinRoomFailedEvent.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {
        //Photon #9
        Debug.Log("Room Controller: Starting Game");
        PhotonNetwork.LoadLevel(PhotonGameSceneIndex);
    }
}
