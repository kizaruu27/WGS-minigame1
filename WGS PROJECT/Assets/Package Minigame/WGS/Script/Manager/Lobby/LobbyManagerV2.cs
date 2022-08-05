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
    public class LobbyManagerV2 : MonoBehaviourPunCallbacks
    {
        public static LobbyManagerV2 instance;

        [Header("Canvas")]
        [SerializeField] Canvas canvas;

        [Header("Lobby")]
        public GameObject lobbyPanel;
        public GameObject menuUI;
        public GameObject searchPlayerPanel;

        [Header("Room")]
        public GameObject roomPanel;
        public TextMeshProUGUI roomName;
        List<RoomItem> roomItemList = new List<RoomItem>();
        public GameObject RoomManager;


        [Header("Player")]
        public List<PlayerAvatar> playerItemsList = new List<PlayerAvatar>();
        public PlayerAvatar playerItemPrefab;
        public Transform playerItemParent;

        [Header("Modal")]
        public TextMeshProUGUI modalTitle;
        public TextMeshProUGUI modalMessage;
        public Button closeModal;
        public GameObject modalPanel;

        [Header("Loading")]
        [SerializeField] GameObject loadingPanel;


        [Header("Utilities")]
        public Transform contentObject;
        public float timeBetweenUpdates = 1.5f;
        float nextUpdateTime;

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

        public void onClickMatchmaking()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = 4;

            // Debug.Log(PhotonNetwork.CountOfPlayersInRooms);

            if (PhotonNetwork.CountOfPlayersInRooms < roomOptions.MaxPlayers)
            {
                Debug.Log("Masuk room");
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
            lobbyPanel.SetActive(false);

            searchPlayerPanel.SetActive(true);
            roomName.text = PhotonNetwork.CurrentRoom.Name;
            UpdatePlayerList();

            // Invoke("loadRoom", 3);
            loadRoom();

            RoomManager.SetActive(true);
        }

        void loadRoom()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 1) // sementara
            {
                roomPanel.SetActive(true);
                searchPlayerPanel.SetActive(false);
            }
        }

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
                foreach (RoomItem item in roomItemList)
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
            lobbyPanel.SetActive(true);
            RoomManager.SetActive(false);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        void UpdatePlayerList()
        {
            foreach (PlayerAvatar item in playerItemsList)
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
                PlayerAvatar newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);  // call the player item
                newPlayerItem.SetPlayerInfo(player.Value);

                if (player.Value == PhotonNetwork.LocalPlayer)
                {
                    newPlayerItem.ApplyLocalChanges();
                }

                playerItemsList.Add(newPlayerItem);
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
