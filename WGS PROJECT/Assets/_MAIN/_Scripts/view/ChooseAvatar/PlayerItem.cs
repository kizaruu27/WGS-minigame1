using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace RunMinigames.View.ChooseAvatar
{
    public class PlayerItem : MonoBehaviourPunCallbacks
    {
        public TextMeshProUGUI playerName;
        public Image backgroundImage;
        public Color highlightColor;
        // public GameObject leftArrowButton;
        // public GameObject rightArrowButton;
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        public Image playerAvatar;
        public Sprite[] avatars;
        public int chooseAvatar;

        [Header("Button")]
        public GameObject objectButton;
        public Button changeButton;


        Player player;

        private void Awake()
        {
            chooseAvatar = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            PlayerPrefs.SetInt("playerAvatar", chooseAvatar);

            playerProperties["playerAvatar"] = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            PhotonNetwork.LocalPlayer.CustomProperties = playerProperties;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }

        private void Start()
        {
            backgroundImage = GetComponent<Image>();
        }

        private void Update()
        {
            changeButton.onClick.AddListener(ShowDisplayCharacter);
        }

        void ShowDisplayCharacter()
        {
            // GameObject display = GameObject.Find("DisplayAvatar");
            // display.SetActive(true);
            Debug.Log("test");
        }

        public void SetPlayerInfo(Player _player)
        {
            playerName.text = _player.NickName;
            player = _player;
            UpdatePlayerItem(player);
        }

        public void ApplyLocalChanges()
        {
            // backgroundImage.color = highlightColor;
            // leftArrowButton.SetActive(true);
            // rightArrowButton.SetActive(true);
            objectButton.SetActive(true);
        }

        public void OnClickLeftArrow()
        {
            chooseAvatar = chooseAvatar == 0 ? avatars.Length - 1 : chooseAvatar - 1;

            playerProperties["playerAvatar"] =
                (int)playerProperties["playerAvatar"] == 0 ?
                    avatars.Length - 1 : (int)playerProperties["playerAvatar"] - 1;

            PlayerPrefs.SetInt("playerAvatar", chooseAvatar);

            PhotonNetwork.LocalPlayer.CustomProperties = playerProperties;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }

        public void OnClickRightArrow()
        {
            chooseAvatar = chooseAvatar == avatars.Length - 1 ? 0 : chooseAvatar + 1;

            playerProperties["playerAvatar"] =
                (int)playerProperties["playerAvatar"] == avatars.Length - 1 ?
                    0 : (int)playerProperties["playerAvatar"] + 1;

            PlayerPrefs.SetInt("playerAvatar", chooseAvatar);

            PhotonNetwork.LocalPlayer.CustomProperties = playerProperties;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (player == targetPlayer)
            {
                UpdatePlayerItem(targetPlayer);
            }
        }

        void UpdatePlayerItem(Player player)
        {

            if (player.CustomProperties.ContainsKey("playerAvatar"))
            {
                playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
                playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
            }
            else
            {
                playerProperties["playerAvatar"] = 0;
            }
        }

    }
}

