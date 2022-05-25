using UnityEngine;
using RunMinigames.Models.Http.PlayerInfo;
using RunMinigames.Services.Http;
using RunMinigames.Services.Photon;
using System.Runtime.InteropServices;
using TMPro;

namespace RunMinigames.Manager.Networking
{
    public class HttpManager : MonoBehaviour
    {

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern string GetToken();
#endif
        public TextMeshProUGUI responseToken;

        //hardcode
        readonly string token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjIiLCJpYXQiOjE2NTI4NTQ4NjEsImV4cCI6MTY4NDQxMTc4N30.WgPvma6Sn6bSgMcB09gCSmTB11np8RQG0ZLkBvB-AZ4";

        // authorization token for WebGL
        string authToken;

        private async void Start()
        {

#if UNITY_WEBGL && !UNITY_EDITOR
            authToken = GetToken();
#endif

            var requestData = new HttpClient(
                HttpConfig.BASE_URL,
                new HttpOptions(),
                token
            );

            var result = await requestData
                .Get<MPlayerInfo>(
                    HttpConfig.ENDPOINT["user"]
                );

            if (result.data.uname.Length > 0)
                GetComponent<PhotonServer>().Connect(result.data.uname);

        }
    }
}