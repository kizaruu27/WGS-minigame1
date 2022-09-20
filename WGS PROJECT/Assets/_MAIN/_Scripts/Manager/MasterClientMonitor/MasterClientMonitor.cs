using UnityEngine;
using Photon.Pun;

namespace RunMinigames.Manager.MasterClientMonitor
{
    public class MasterClientMonitor : MonoBehaviourPunCallbacks
    {
        double lastSavedTimeStamp;

        public override void OnJoinedRoom()
        {
            ChangeHost();
        }
        public void ChangeHost()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                lastSavedTimeStamp += Time.deltaTime;
                if (PhotonNetwork.Time - lastSavedTimeStamp > 1)
                {
                    lastSavedTimeStamp = 0;
                    this.photonView.RPC("CheckNetworkLag", PhotonNetwork.MasterClient.GetNext());
                }
            }
            else if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient.GetNext())
            {
                lastSavedTimeStamp += Time.deltaTime;
                {
                    if (PhotonNetwork.Time - lastSavedTimeStamp > 2)
                    {
                        lastSavedTimeStamp = 0;
                        PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
                    }
                }
            }
        }

        [PunRPC]
        public void CheckNetworkLag()
        {
            lastSavedTimeStamp = PhotonNetwork.Time;
        }
        public override void OnMasterClientSwitched(Photon.Realtime.Player newMaster)
        {
            if (newMaster == PhotonNetwork.LocalPlayer)
                lastSavedTimeStamp = 1;
            else if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient.GetNext())
                lastSavedTimeStamp = -3;
        }
    }
}