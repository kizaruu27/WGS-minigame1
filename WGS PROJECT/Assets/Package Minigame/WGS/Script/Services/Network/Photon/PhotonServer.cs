using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace RunMinigames.Services.Photon
{

    public class PhotonServer : MonoBehaviourPunCallbacks
    {

        public static PhotonServer instance;

        private void Start() => instance = this;

        public void Connect(string uname)
        {
            if (uname.Length > 0)
            {
                PhotonNetwork.NickName = uname;
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.ConnectUsingSettings();
                PlayerPrefs.SetString("LocalPlayerNickname", uname);
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log($"Connected to master {PhotonNetwork.NickName}");
            SceneManager.LoadScene("WGS1_GameMenu");
        }

        public override void OnConnected()
        {
            base.OnConnected();
            Debug.Log("Connected");
        }
    }
}