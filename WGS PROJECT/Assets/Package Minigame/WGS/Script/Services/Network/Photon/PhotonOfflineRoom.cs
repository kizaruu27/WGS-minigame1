using Photon.Pun;
using Photon.Realtime;

namespace RunMinigames.Services.Photon
{
    public class PhotonOfflineRoom : MonoBehaviourPunCallbacks
    {
        public void OnCreateRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.CreateRoom("Offline Room", new RoomOptions() { MaxPlayers = 1, BroadcastPropsChangeToAll = true });
            }
        }
    }
}