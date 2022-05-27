using UnityEngine;
using RunMinigames.Models.Http;
using RunMinigames.Models.Http.PlayerInfo;
using RunMinigames.Services.Http;
using RunMinigames.Services.Photon;
using RunMinigames.View.Loading;
using System.Runtime.InteropServices;
using System;
using Photon.Pun;
using TMPro;

namespace RunMinigames.Manager.Networking
{
    public class HttpManager : MonoBehaviour
    {


        [DllImport("__Internal")]
        private static extern string GetToken();

        public TextMeshProUGUI responseToken;
        private bool deviceType;
        private MHttpResponse<MPlayerInfo>? result;

        //development token
        readonly string localToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjIiLCJpYXQiOjE2NTI4NTQ4NjEsImV4cCI6MTY4NDQxMTc4N30.WgPvma6Sn6bSgMcB09gCSmTB11np8RQG0ZLkBvB-AZ4";

        // authorization token for WebGL
        string authToken;

        private void Start()
        {
            var urlToken = GetToken();
            deviceType = CheckPlatform.isWeb && (!CheckPlatform.isMacUnity || !CheckPlatform.isWindowsUnity);
            authToken = deviceType ? $"Bearer {urlToken}" : localToken;
        }

        private void Update()
        {
            Login();
        }


        async void Login()
        {
            try
            {
                if (result is null)
                {
                    var requestData = new HttpClient(
                        HttpConfig.BASE_URL,
                        new HttpOptions(),
                        authToken
                    );

                    result = await requestData.Get<MPlayerInfo>(HttpConfig.ENDPOINT["user"]);

                    LoginStatus.instance.StepperMessage("Get User Data");
                    LoginStatus.instance.isConnectingToServer = false;
                }

                if (result?.response.data.uname.Length > 0)
                {
                    PlayerPrefs.SetString("token", authToken);
                    PlayerPrefs.SetString("LocalPlayerNickname", result?.response.data.uname);

                    GetComponent<PhotonServer>().Connect(PlayerPrefs.GetString("LocalPlayerNickname"));
                }

            }
            catch (Exception error)
            {
                Debug.LogError($"failed: {error.Message}");
            }
        }
    }
}