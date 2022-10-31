using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using RunMinigames.View.ChooseAvatar;


namespace RunMinigames.Manager.Lobby
{
    public class M1_LobbyManager : MonoBehaviourPunCallbacks
    {
        [Header("Canvas")]
        [SerializeField] Canvas canvas;

        [Header("Lobby")]
        public GameObject lobbyPanel;
        public GameObject menuUI;
        public GameObject searchPlayerPanel;

        [Header("Room")]
        public TMP_InputField roomInputField;
        public GameObject roomPanel;
        public TextMeshProUGUI roomName;

        public M1_RoomItem m1RoomItemPrefab;
        List<M1_RoomItem> roomItemList = new List<M1_RoomItem>();

        [Header("Player")]
        public List<M1_PlayerItem> playerItemsList = new List<M1_PlayerItem>();
        public M1_PlayerItem m1PlayerItemPrefab;
        public Transform playerItemParent;

        [Header("Modal")]
        public TextMeshProUGUI modalTitle;
        public TextMeshProUGUI modalMessage;
        public Button closeModal;
        public GameObject modalPanel;

        [Header("Loading")]
        [SerializeField] GameObject loadingPanel;


        [Header("Utilities")]
        public GameObject playButton;
        public Transform contentObject;
        public float timeBetweenUpdates = 1.5f;
        float nextUpdateTime;

        private void Start()
        {
            // PhotonNetwork.JoinLobby();
            modalPanel.SetActive(false);
        }

        private void Update()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable || !PhotonNetwork.IsConnected)
            {
                Modal("Connection Error", " Check internet connection!");
            }

            Invoke("loadRoom", 10);
            // loadRoom();

            playButton.SetActive(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1);
        }

        public void OnClickCreate()
        {
            if (roomInputField.text.Length >= 1)
            {
                loadingPanel.SetActive(true);

                if (PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 4, BroadcastPropsChangeToAll = true });
                }
                else
                {
                    Modal("Not Connected", "Please Check Your Internet Connection");

                }
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
            PhotonNetwork.JoinOrCreateRoom("Game", roomOptions, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            loadingPanel.SetActive(false);
            lobbyPanel.SetActive(false);

            searchPlayerPanel.SetActive(true);
            roomName.text = PhotonNetwork.CurrentRoom.Name;
            UpdatePlayerList();
        }

        void loadRoom()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1) // sementara
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
                foreach (M1_RoomItem item in roomItemList)
                {
                    Destroy(item.gameObject);
                }
                roomItemList.Clear();
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            if (Time.time >= nextUpdateTime)
            {
                UpdateRoomList(roomList);
                nextUpdateTime = Time.time + timeBetweenUpdates;
            }
        }

        void UpdateRoomList(List<RoomInfo> list)
        {
            foreach (M1_RoomItem item in roomItemList)
            {
                if (item.gameObject.name != null)
                    Destroy(item.gameObject);
            }
            roomItemList.Clear();

            foreach (RoomInfo room in list)
            {
                M1_RoomItem newM1Room = Instantiate(m1RoomItemPrefab, contentObject);
                newM1Room.SetRoomName(room.Name);
                newM1Room.roomInfo = room;

                if (!room.IsOpen || room.RemovedFromList)
                {
                    roomItemList.Remove(newM1Room);
                }

                roomItemList.Add(newM1Room);

            }
        }

        public void JoinRoom(string roomName)
        {
            loadingPanel.SetActive(true);
            PhotonNetwork.JoinRoom(roomName);
        }

        public void OnClickLeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            roomPanel.SetActive(false);
            lobbyPanel.SetActive(true);
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        void UpdatePlayerList()
        {
            foreach (M1_PlayerItem item in playerItemsList)
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
                M1_PlayerItem newM1PlayerItem = Instantiate(m1PlayerItemPrefab, playerItemParent);  // call the player item
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


        public void OnClickPlayButton(string targetScene)
        {
            PhotonNetwork.LoadLevel(targetScene);
        }

        public void OnClickDisconnect()
        {
            PhotonNetwork.Disconnect();
        }

    }
}
