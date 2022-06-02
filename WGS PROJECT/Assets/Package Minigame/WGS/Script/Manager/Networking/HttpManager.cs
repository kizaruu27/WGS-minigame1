using UnityEngine;
using RunMinigames.Models.Http.PlayerInfo;
using RunMinigames.Services.Http;
using RunMinigames.Services.Photon;
using RunMinigames.View.Loading;
using System;
using System.Runtime.InteropServices;


namespace RunMinigames.Manager.Networking
{
    public class HttpManager : MonoBehaviour
    {

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string GetToken();
#endif

        private bool deviceType;
        private MPlayerInfo result;

        //development token
        readonly string localToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjIiLCJpYXQiOjE2NTI4NTQ4NjEsImV4cCI6MTY4NDQxMTc4N30.WgPvma6Sn6bSgMcB09gCSmTB11np8RQG0ZLkBvB-AZ4";

        // authorization token for WebGL
        string authToken;
        string urlToken;
        HttpClientV2 client;

        private void Start()
        {

#if UNITY_WEBGL && !UNITY_EDITOR
            urlToken = "Bearer " + GetToken();
#endif

            deviceType = CheckPlatform.isWeb && (!CheckPlatform.isMacUnity || !CheckPlatform.isWindowsUnity);
            authToken = deviceType ? urlToken : localToken;


            client = new HttpClientV2(
                        HttpConfig.BASE_URL,
                        new HttpOptions(),
                        authToken
                    );

            Debug.Log(client);

        }

        private void Update()
        {
            // Login();
            Debug.Log(client);


            if (client != null)
            {
                StartCoroutine(client.Get(
                    HttpConfig.ENDPOINT["user"],
                    (result) => Debug.Log(result["data"])
                ));
            }

        }


        void LoginV2()
        {
            if (result is null)
            {

            }
        }


        async void Login()
        {
            try
            {
                if (result is null)
                {
                    Debug.Log("from login : " + authToken);
                    var requestData = new HttpClient(
                        HttpConfig.BASE_URL,
                        new HttpOptions(),
                        authToken
                    );

                    result = await requestData.Get<MPlayerInfo>(
                        HttpConfig.ENDPOINT["user"],
                        (isDone, downloadProgress) =>
                            LoginStatus.instance.StepperMessage(downloadProgress < 100 && !isDone ? "Getting user data..." : "process user data...")
                    );

                    Debug.Log("http result : " + result?.data.uname);


                    LoginStatus.instance.isConnectingToServer = false;
                }


                if (result?.data.uname.Length > 0)
                {
                    PlayerPrefs.SetString("token", authToken);
                    PlayerPrefs.SetString("LocalPlayerNickname", result?.data.uname);

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