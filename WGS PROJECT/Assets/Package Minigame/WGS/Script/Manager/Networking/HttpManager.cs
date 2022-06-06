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

        readonly string EditorToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjIiLCJpYXQiOjE2NTI4NTQ4NjEsImV4cCI6MTY4NDQxMTc4N30.WgPvma6Sn6bSgMcB09gCSmTB11np8RQG0ZLkBvB-AZ4";
        readonly string BuildToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjQiLCJpYXQiOjE2NTM2MjE0NzIsImV4cCI6MTY4NTE3ODM5OH0.PsKoprNpr3sudUxukyYA58d1Hx6amSWWIOj4YERBMGQ";
        string localToken;

        // authorization token for WebGL
        string authToken;
        string urlToken;
        HttpClientV2 client;

        bool isStopRequest;
        JSONNode data;
        Scene currScene;

        private void Awake()
        {

            GetToken();

            client = new HttpClientV2(
                        HttpConfig.BASE_URL,
                        new HttpOptions(),
                        authToken
                    );

            currScene = SceneManager.GetActiveScene();
        }

        private void Update()
        {
            if (currScene.name == "WGS1_Login" && !isStopRequest) GetUserData();
        }


        void GetToken()
        {

#if UNITY_WEBGL && !UNITY_EDITOR
            urlToken = "Bearer " + GetToken();
#endif
            localToken = (CheckPlatform.isMacUnity || CheckPlatform.isWindowsUnity) ? EditorToken : BuildToken;
            deviceType = CheckPlatform.isWeb && (!CheckPlatform.isMacUnity || !CheckPlatform.isWindowsUnity);
            authToken = deviceType ? urlToken : localToken;
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

                    GetComponent<PhotonServer>().Connect(JSON.Parse(PlayerPrefs.GetString("LocalPlayerData"))["full_name"]);
                }
            }));
        }
    }
}