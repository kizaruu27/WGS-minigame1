using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace RunMinigames.Services.Photon
{
    public class PhotonOfflineRoom : MonoBehaviourPunCallbacks
    {
        private void Awake()
        {
            Debug.Log(PhotonNetwork.InLobby);
        }
    }
}