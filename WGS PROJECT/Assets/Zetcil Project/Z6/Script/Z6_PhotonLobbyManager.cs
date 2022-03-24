using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Z6_PhotonLobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Active Button")]
    public GameObject ButtonConnect;
    public GameObject ButtonCreateRoom;
    public GameObject ButtonJoinRoom;
    public int RoomSize;

    [Header("Create Room")]
    public InputField CreateRoomName;
    public UnityEvent CreateRoomSuccessEvent;
    public UnityEvent CreateRoomFailedEvent;

    [Header("Join Room")]
    public InputField JoinRoomName;
    public UnityEvent JoinRoomSuccessEvent;
    public UnityEvent JoinRoomFailedEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        //Photon #3
        PhotonNetwork.AutomaticallySyncScene = true;
        ButtonConnect.SetActive(false);
        ButtonCreateRoom.SetActive(true);
        ButtonJoinRoom.SetActive(true);
    }

    public void CreateRoom()
    {
        //Photon #4
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom(CreateRoomName.text, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //Photon #5
        CreateRoomFailedEvent.Invoke();
        Debug.Log("Lobby Manager: Create Room Failed");
    }

    public override void OnCreatedRoom()
    {
        //Photon #6
        CreateRoomFailedEvent.Invoke();
        Debug.Log("Lobby Manager: Create Room Success");
    }

    public override void OnJoinedRoom()
    {
        //Photon #7
        Debug.Log("Room Controller: Joined Room");
        JoinRoomSuccessEvent.Invoke();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //Photon #8
        Debug.Log("Room Controller: " + message);
        JoinRoomFailedEvent.Invoke();
    }


    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(JoinRoomName.text);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

}
