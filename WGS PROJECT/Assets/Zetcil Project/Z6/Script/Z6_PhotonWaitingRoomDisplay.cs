using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Z6_PhotonWaitingRoomDisplay : MonoBehaviourPunCallbacks
{
    private PhotonView TargetPhotonView;

    [Header("Network Scene Index")]
    public int MatchmakingSceneIndex;
    public int PhotonGameSceneIndex;

    [Header("UI")]
    public GameObject ButtonStartGame;
    public Text PlayerCountDisplay;

    int PlayerCount;
    int RoomSize;

    // Start is called before the first frame update
    void Start()
    {
        TargetPhotonView = GetComponent<PhotonView>();
        PlayerCountUpdate();
        if (PhotonNetwork.IsMasterClient)
        {
            ButtonStartGame.SetActive(true);
        } else
        {
            ButtonStartGame.SetActive(false);
        }
    }

    void PlayerCountUpdate()
    {
        PlayerCount = PhotonNetwork.PlayerList.Length;
        RoomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        PlayerCountDisplay.text = PlayerCount.ToString() + " / " + RoomSize.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(PhotonGameSceneIndex);
    }

    public void LeaveRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(MatchmakingSceneIndex);
        }
        else 
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(MatchmakingSceneIndex);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        PlayerCountUpdate();
    }

}
