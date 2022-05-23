using UnityEngine;
using RunMinigames.Models;

namespace RunMinigames.Services
{
    public class MTest
    {

    }

    public class HttpManager : MonoBehaviour
    {

        // <summary>
        // private async void Start()
        // {
        //     var requestData = new HttpClient(HttpConfig.baseurl);
        //     var data = await requestData.Get<MTest>(HttpConfig.endpoint["user"]);
        // }
        // </summary>


        string token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjIiLCJpYXQiOjE2NTI4NTQ4NjEsImV4cCI6MTY4NDQxMTc4N30.WgPvma6Sn6bSgMcB09gCSmTB11np8RQG0ZLkBvB-AZ4";

        private async void Start()
        {
            var requestData = new HttpClient(
                HttpConfig.baseurl,
                new JsonSerializationOption(),
                token
            );

            var data = await requestData.Get<MPlayerInfo>(HttpConfig.endpoint["user"]);
            Debug.Log(data.data);
        }
    }
}