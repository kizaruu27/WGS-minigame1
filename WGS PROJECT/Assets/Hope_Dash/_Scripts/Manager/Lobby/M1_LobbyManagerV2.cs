using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using RunMinigames.View.PlayerAvatar;
using RunMinigames.Manager.Room;

namespace RunMinigames.Manager.Lobby
{
  public class M1_LobbyManagerV2 : MonoBehaviourPunCallbacks
  {
    public static M1_LobbyManagerV2 instance;

    [Header("Lobby")]
    public GameObject searchPlayerPanel;

    [Header("Room")]
    public GameObject roomPanel;
    public TextMeshProUGUI roomName;
    List<M1_RoomItem> roomItemList = new List<M1_RoomItem>();

    [Header("Player")]
    public List<M1_PlayerAvatar> playerItemsList = new List<M1_PlayerAvatar>();
    public M1_PlayerAvatar m1PlayerItemPrefab;
    public Transform playerItemParent;

    [Header("Modal")]
    public TextMeshProUGUI modalTitle;
    public TextMeshProUGUI modalMessage;
    public GameObject modalPanel;

    [Header("Loading")]
    [SerializeField] GameObject loadingPanel;

    [SerializeField] int roomIndex;

    private void Awake() => instance = this;
    private void Start()
    {
      modalPanel.SetActive(false);
    }

    private void Update()
    {
      if (Application.internetReachability == NetworkReachability.NotReachable || !PhotonNetwork.IsConnected)
      {
        Modal("Connection Error", " Check internet connection!");
      }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
      Modal("Failed To Create Room", message);
    }

    public void OnCloseModal()
    {
      modalPanel.SetActive(false);
    }


    #region Matchmaking
    public void onClickMatchmaking()
    {
      RoomOptions roomOptions = new RoomOptions();
      roomOptions.IsVisible = true;
      roomOptions.MaxPlayers = 4;

      // Debug.Log(PhotonNetwork.CountOfPlayersInRooms);

      if (PhotonNetwork.CountOfPlayersInRooms < roomOptions.MaxPlayers)
      {
        PhotonNetwork.JoinOrCreateRoom("Room " + roomIndex.ToString(), roomOptions, TypedLobby.Default);
      }
      else
      {
        PhotonNetwork.LeaveRoom();
        roomIndex++;
        PhotonNetwork.JoinOrCreateRoom("Room " + roomIndex.ToString(), roomOptions, TypedLobby.Default);
      }
    }

    public override void OnJoinedRoom()
    {
      loadingPanel.SetActive(false);

      searchPlayerPanel.SetActive(true);
      roomName.text = PhotonNetwork.CurrentRoom.Name;
      UpdatePlayerList();

      // Invoke("loadRoom", 3);
      loadRoom();

      M1_RoomManager.instance.GetCurrentRoomPlayers();
    }

    void loadRoom()
    {
      if (PhotonNetwork.CurrentRoom.PlayerCount >= 1) // sementara
      {
        roomPanel.SetActive(true);
        searchPlayerPanel.SetActive(false);
      }
    }

    void ActivateRoom()
    {
      roomPanel.SetActive(true);
      searchPlayerPanel.SetActive(false);
    }
    #endregion


    public void Modal(string title, string message)
    {
      modalPanel.SetActive(true);
      loadingPanel.SetActive(false);
      modalTitle.text = title;
      modalMessage.text = message;
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
      Modal("Failed To Join Room", message);

      if (message.ToLower() == "game does not exist")
      {
        foreach (M1_RoomItem item in roomItemList)
        {
          Destroy(item.gameObject);
        }
        roomItemList.Clear();
      }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

    }

    public void OnClickLeaveRoom()
    {
      PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
      roomPanel.SetActive(false);

      M1_RoomManager.instance.GetCurrentRoomPlayers();
    }

    public override void OnConnectedToMaster()
    {
      PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList()
    {
      foreach (M1_PlayerAvatar item in playerItemsList)
      {
        Destroy(item.gameObject);
      }
      playerItemsList.Clear();

      if (PhotonNetwork.CurrentRoom == null)
      {
        return;
      }

      foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
      {
        M1_PlayerAvatar newM1PlayerItem = Instantiate(m1PlayerItemPrefab, playerItemParent);  // call the player item
        newM1PlayerItem.SetPlayerInfo(player.Value);

        if (player.Value == PhotonNetwork.LocalPlayer)
        {
          newM1PlayerItem.ApplyLocalChanges();
        }

        playerItemsList.Add(newM1PlayerItem);
      }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
      UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
      UpdatePlayerList();
    }

    public void OnClickModalDisconnect()
    {
      if (Application.internetReachability == NetworkReachability.NotReachable || !PhotonNetwork.IsConnected)
        OnClickDisconnect();
    }

    public void OnClickDisconnect()
    {
      PhotonNetwork.Disconnect();
    }
  }
}
