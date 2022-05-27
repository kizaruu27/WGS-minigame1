using UnityEngine;
using Photon.Pun;
using TMPro;
using RunMinigames.Manager.Game;

namespace RunMinigames.View.Countdown
{
    public class CountdownStart : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _CountdownTXT;
        public float InitialCountdown = 4f;

        PhotonView view;
        GameManager gameManager;

        private void Awake()
        {
            view = GetComponent<PhotonView>();
            gameManager = GameObject.FindGameObjectWithTag(nameof(GameManager)).GetComponent<GameManager>();
        }

        private void Update() => StartCoroutine(gameManager.WaitAllPlayerReady(StartCountdown));

        public void StartCountdown()
        {
            InitialCountdown -= Time.deltaTime;

            if (PhotonNetwork.IsMasterClient)
                view.RPC(nameof(SetCountdown), RpcTarget.Others, InitialCountdown);

            var isStart = InitialCountdown <= 0f;

            _CountdownTXT.fontSize = InitialCountdown > 2 ? 200f : 80f;
            _CountdownTXT.text = InitialCountdown > 2 ? $"{(int)(InitialCountdown - 1)}" : "Start";

            gameObject.SetActive(!isStart);

            if (isStart)
                gameManager.SetActiveCharacter();
        }

        [PunRPC]
        void SetCountdown(float newCountdown) => InitialCountdown = newCountdown;
    }
}