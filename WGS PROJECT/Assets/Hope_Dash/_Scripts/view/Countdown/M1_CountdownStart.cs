using UnityEngine;
using Photon.Pun;
using TMPro;
using RunMinigames.Manager.Game;

namespace RunMinigames.View.Countdown
{
    public class M1_CountdownStart : MonoBehaviour
    {
        [Header("Item")]
        [SerializeField] TextMeshProUGUI _CountdownTXT;
        public float InitialCountdown = 4f;
        bool isStart;


        [Header("Component")]
        [SerializeField] PhotonView photonView;
        [SerializeField] M1_GameManager m1GameManager;

        private void Update() => StartCoroutine(m1GameManager.WaitAllPlayerReady(StartCountdown));

        public void StartCountdown()
        {
            InitialCountdown -= Time.deltaTime;

            if (PhotonNetwork.IsMasterClient)
                photonView.RPC(nameof(SetCountdown), RpcTarget.Others, InitialCountdown);

            isStart = InitialCountdown <= 0f;

            _CountdownTXT.fontSize = InitialCountdown > 2 ? 200f : 80f;
            _CountdownTXT.text = InitialCountdown > 2 ? $"{(int)(InitialCountdown - 1)}" : "Start";

            gameObject.SetActive(!isStart);

            if (isStart)
                m1GameManager.SetActiveCharacter();

        }

        [PunRPC]
        void SetCountdown(float newCountdown) => InitialCountdown = newCountdown;
    }
}