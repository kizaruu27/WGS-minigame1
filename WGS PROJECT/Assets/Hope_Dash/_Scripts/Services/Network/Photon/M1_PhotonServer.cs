using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using RunMinigames.Manager.Networking;
using RunMinigames.View.Loading;
using System.Text.RegularExpressions;

namespace RunMinigames.Services.Photon
{

    public class M1_PhotonServer : MonoBehaviourPunCallbacks
    {

        public static M1_PhotonServer instance;

        private void Start() => instance = this;

        public void Connect(string uname)
        {
            M1_LoginStatus.instance.StepperMessage(
                Regex.Replace(
                    PhotonNetwork.NetworkClientState.ToString(), "([A-Z])", " $1", RegexOptions.Compiled
                ).Trim()
            );

            M1_LoginStatus.instance.isConnectingToServer = true;

            if (uname.Length > 0)
            {
                PhotonNetwork.NickName = uname;
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            var httpManager = GetComponent<M1_HttpManager>();
            httpManager.enabled = false;

            if (!httpManager.enabled)
            {
                Destroy(httpManager.gameObject);
                PhotonNetwork.JoinLobby();
                SceneManager.LoadScene("WGS2_Lobby");
            }

        }
    }
}