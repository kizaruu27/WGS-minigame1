using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Z6_PhotonWaitingRoomController : MonoBehaviourPunCallbacks
{
    public int WaitingRoomSceneIndex;

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
        WaitingRoom();
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

    void WaitingRoom()
    {
        //Photon #9
        Debug.Log("Room Controller: Waiting Room");
        PhotonNetwork.LoadLevel(WaitingRoomSceneIndex);
    }
}
