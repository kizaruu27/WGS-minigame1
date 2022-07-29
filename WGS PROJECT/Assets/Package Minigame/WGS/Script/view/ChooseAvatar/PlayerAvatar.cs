using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using RunMinigames.Manager.Room;

namespace RunMinigames.View.PlayerAvatar
{
    public class PlayerAvatar : MonoBehaviourPunCallbacks
    {
        [Header("Component")]
        public TextMeshProUGUI playerName;
        public Image playerAvatar;
        public Sprite[] avatars;
        public int avatarIndex;

        [Header("Change Properties")]
        public GameObject objectButton;
        public int _AvIndex;

        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        Player player;

        private void Awake()
        {
            avatarIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            PlayerPrefs.SetInt("playerAvatar", avatarIndex);

            playerProperties["playerAvatar"] = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            PhotonNetwork.LocalPlayer.CustomProperties = playerProperties;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }

        public void OnClickShowDisplayAvatar()
        {
            RoomManager.instance.DisplayAvaParent.SetActive(true);
            RoomManager.instance.AvaToggleGroup.SetAllTogglesOff();
        }

        public void OnClickGetAvatarIndex(int _index)
        {
            _AvIndex = _index;
        }

        public void OnClickSetAvatarIndex()
        {
            avatarIndex = _AvIndex;

            playerProperties["playerAvatar"] = (int)_AvIndex;

            PlayerPrefs.SetInt("playerAvatar", avatarIndex);
            PhotonNetwork.LocalPlayer.CustomProperties = playerProperties;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);

            RoomManager.instance.DisplayAvaParent.SetActive(false);
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
