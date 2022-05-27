using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using RunMinigames.Manager.Networking;

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
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log($"Connected to master {PhotonNetwork.NickName}");

            var httpManager = GetComponent<HttpManager>();
            httpManager.enabled = false;

            if (!httpManager.enabled)
            {
                Destroy(httpManager.gameObject);
                SceneManager.LoadScene("WGS2_GameMenu");
            }

        }

        public override void OnConnected()
        {
            base.OnConnected();
            Debug.Log("Connected");
        }
    }
}