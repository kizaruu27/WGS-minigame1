using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;


namespace RunMinigames.Manager.Room
{
    public class RoomManager : MonoBehaviour
    {
        public static RoomManager instance;

        [Header("Timer Components")]
        public TextMeshProUGUI TextTimer;
        public float CooldownTime = 10;
        private float Timer;
        public bool allPlayerReady;


        [Header("Choose Avatar Components")]
        public GameObject DisplayAvaParent;
        public ToggleGroup AvaToggleGroup;

        private void Awake() => instance = this;

        void SetText()
        {
            TextTimer.text = Timer.ToString("0");
        }

        private void Update()
        {
            StartCountDown();

            SetText();
        }

        public void SetAllPlayerReady()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
                allPlayerReady = true;
        }

        float s = 0;
        void StartCountDown()
        {

            if (!allPlayerReady)
            {
                Timer = CooldownTime;
                TextTimer.enabled = false;
            }
            else
            {
                TextTimer.enabled = true;
                s += Time.deltaTime;
                if (s >= 1)
                {
                    Timer--;
                    s = 0;
                }
            }

            if (allPlayerReady && Timer == 0)
            {
                Debug.Log("Pindah Scene");
                allPlayerReady = false;
            }

        }
    }
}
