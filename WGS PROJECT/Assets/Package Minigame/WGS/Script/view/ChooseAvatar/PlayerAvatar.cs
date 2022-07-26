using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using RunMinigames.Manager.Lobby;

namespace RunMinigames.View.PlayerAvatar
{
    public class PlayerAvatar : MonoBehaviourPunCallbacks
    {
        [Header("Component")]
        public TextMeshProUGUI playerName;
        public Image playerAvatar;
        public Sprite[] avatars;
        public int chooseAvatar;

        [Header("Button Change")]
        public GameObject objectButton;

        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        Player player;

        private void Awake()
        {
            chooseAvatar = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            PlayerPrefs.SetInt("playerAvatar", chooseAvatar);

            playerProperties["playerAvatar"] = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            PhotonNetwork.LocalPlayer.CustomProperties = playerProperties;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }

        public void OnClickShowDisplayAvatar()
        {
            LobbyManagerV2.instance.DisplayAvatars.SetActive(true);
        }

        public void OnClickChangeAvatar(int _index)
        {
            chooseAvatar = _index;

            playerProperties["playerAvatar"] = (int)_index;

            PlayerPrefs.SetInt("playerAvatar", chooseAvatar);
            PhotonNetwork.LocalPlayer.CustomProperties = playerProperties;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);

            LobbyManagerV2.instance.DisplayAvatars.SetActive(false);
        }

        public void SetPlayerInfo(Player _player)
        {
            playerName.text = _player.NickName;
            player = _player;
            UpdatePlayerItem(player);
        }

        public void ApplyLocalChanges()
        {
            objectButton.SetActive(true);
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
