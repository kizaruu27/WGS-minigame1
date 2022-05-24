using UnityEngine;
using RunMinigames.Models.Http.PlayerInfo;
using RunMinigames.Services;
using System.Runtime.InteropServices;
using TMPro;

namespace RunMinigames.Manager.Networking
{
    public class HttpManager : MonoBehaviour
    {

#if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void Hello();

        [DllImport("__Internal")]
        private static extern string GetToken();

        [DllImport("__Internal")] private static extern string GetBaseURL();
#endif

        public TextMeshProUGUI responseToken;

        //hardcode
        readonly string token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjIiLCJpYXQiOjE2NTI4NTQ4NjEsImV4cCI6MTY4NDQxMTc4N30.WgPvma6Sn6bSgMcB09gCSmTB11np8RQG0ZLkBvB-AZ4";

        private async void Start()
        {
            // DisplayToken();

            var requestData = new HttpClient(
                HttpConfig.BASE_URL,
                new HttpOptions(),
                token
            );

            var result = await requestData
                .Get<MPlayerInfo>(
                    HttpConfig.ENDPOINT["user"]
                );

            Debug.Log(result.data.uname);
        }

#if UNITY_WEBGL
        public void DisplayToken()
        {

            responseToken.text = GetBaseURL();
        }
#endif
    }
}