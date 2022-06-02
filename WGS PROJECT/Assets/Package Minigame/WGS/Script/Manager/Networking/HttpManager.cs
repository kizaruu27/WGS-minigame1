using UnityEngine;
using UnityEngine.SceneManagement;
using RunMinigames.Models.Http.PlayerInfo;
using RunMinigames.Services.Http;
using RunMinigames.Services.Photon;
using RunMinigames.View.Loading;
using System;
using System.Runtime.InteropServices;
using SimpleJSON;


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
        readonly string localToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjQiLCJpYXQiOjE2NTM0NjE0MzAsImV4cCI6MTY4NTAxODM1Nn0.8dZQM-e5hrfEZTHGESWjANskD8Tmn3oCYAgrmoBUccM";

        // authorization token for WebGL
        string authToken;
        string urlToken;
        HttpClientV2 client;

        bool isStopRequest;
        JSONNode data;
        Scene currScene;

        private void Awake()
        {

#if UNITY_WEBGL && !UNITY_EDITOR
            urlToken = "Bearer " + GetToken();
#endif

            deviceType = CheckPlatform.isWeb && (!CheckPlatform.isMacUnity || !CheckPlatform.isWindowsUnity);
            authToken = deviceType ? urlToken : localToken;

            currScene = SceneManager.GetActiveScene();

            client = new HttpClientV2(
                        HttpConfig.BASE_URL,
                        new HttpOptions(),
                        authToken
                    );
        }

        private void Update()
        {
            if (currScene.name == "WGS1_Login" && !isStopRequest) GetUserData();
        }


        void GetUserData()
        {
            LoginStatus.instance.StepperMessage("Getting user data...");
            StartCoroutine(client.Get(HttpConfig.ENDPOINT["user"], (res) =>
            {
                isStopRequest = res != null;
                LoginStatus.instance.StepperMessage("process user data...");

                if (isStopRequest)
                {
                    PlayerPrefs.SetString("token", authToken);
                    PlayerPrefs.SetString("LocalPlayerData", res["data"].ToString());

                    GetComponent<PhotonServer>().Connect(JSON.Parse(PlayerPrefs.GetString("LocalPlayerData"))["uname"]);
                }
            }));
        }
    }
}