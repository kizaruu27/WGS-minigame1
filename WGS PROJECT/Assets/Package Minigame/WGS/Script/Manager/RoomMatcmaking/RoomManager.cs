using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using RunMinigames.View.PlayerAvatar;
namespace RunMinigames.Manager.Room
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        public static RoomManager instance;
        bool GameStart = false;
        public Button readyButton;

        [Header("Timer Components")]
        public TextMeshProUGUI TextTimer;
        public double CooldownTime = 30;
        private double Timer;
        [SerializeField] bool startTimer = false;
        double startTime;
        ExitGames.Client.Photon.Hashtable CustomeValue = new ExitGames.Client.Photon.Hashtable();
        public int playerReadyCount;

        [Header("Choose Avatar Components")]
        public GameObject DisplayAvaParent;
        public ToggleGroup AvaToggleGroup;

        [Header("Scenes")]
        public string[] sceneName;

        [Header("PlayerList")]
        public List<string> playersList = new List<string>();

        private void Awake()
        {
            instance = this;
            GameStart = false;
            GetCurrentRoomPlayers();
        }

        void SetText(double _timer)
        {
            TextTimer.text = _timer.ToString("0");
        }

        private void Update() => WaitingRoomControl();


        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            GetCurrentRoomPlayers();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            photonView.RPC("RPC_SetReadyState", RpcTarget.AllBufferedViaServer, false);

            GetCurrentRoomPlayers();
        }

        public void WaitingRoomControl()
        {
            if (GameStart != true)
            {
                StartCountDown();

                if (PhotonNetwork.IsMasterClient)
                {
                    if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
                    {
                        startTimer = false;
                        SetText(CooldownTime);
                    }
                    else if (PhotonNetwork.CurrentRoom.PlayerCount > 1 && startTimer == false)
                    {
                        SetStartTime();
                    }

                    if (PhotonNetwork.CurrentRoom.PlayerCount > 0 && playerReadyCount == PhotonNetwork.CurrentRoom.PlayerCount)
                        StartGame();
                }
                else
                {
                    SetStartTimeClient();
                }
            }
        }

        public void GetCurrentRoomPlayers()
        {
            playersList.Clear();

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                playersList.Add(player.NickName);
            }

            SetPlayerSpawnIndex();
        }

        void SetStartTime()
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                startTime = PhotonNetwork.Time;
                startTimer = true;

                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("StartTime"))
                {
                    CustomeValue["StartTime"] = startTime;
                    PhotonNetwork.LocalPlayer.CustomProperties = CustomeValue;
                    PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
                }
                else
                {
                    CustomeValue.Add("StartTime", startTime);
                    PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
                }
            }
        }

        void SetStartTimeClient()
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("StartTime"))
            {
                startTime = (double)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
                startTimer = true;
            }
        }

        void StartCountDown()
        {
            if (startTimer == false)
                return;

            Timer = CooldownTime - (PhotonNetwork.Time - startTime);
            SetText(Timer);

            if (Timer < 0)
            {
                SetText(0);
                startTimer = false;

                StartGame();
            }
        }

        public void SetPlayerSpawnIndex()
        {
            for (int i = 0; i < playersList.Count; i++)
            {
                if (playersList[i] == PhotonNetwork.LocalPlayer.NickName)
                {
                    int playerIndex = playersList.IndexOf(playersList[i]);
                    PlayerPrefs.SetInt("positionIndex", playerIndex);
                }
            }
        }

        public void StartGame()
        {
            GameStart = true;

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.LoadLevel(sceneName[Random.Range(0, sceneName.Length)]);
            }
        }

        public void SetPlayerReady()
        {
            if (PhotonNetwork.LocalPlayer.IsLocal)
            {
                readyButton.interactable = false;
                photonView.RPC("RPC_SetReadyState", RpcTarget.AllBufferedViaServer, true);
            }
        }

        [PunRPC]
        public void RPC_SetReadyState(bool add = true)
        {
            if (add)
            {
                playerReadyCount++;
            }
            else
            {
                playerReadyCount = 0;
                readyButton.interactable = true;
            }
        }
    }
}
