using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using RunMinigames.Interface;
using SimpleJSON;
using Newtonsoft.Json;
using RunMinigames.Models.Http.PlayerInfo;

namespace RunMinigames.Services.Http
{
    public class HttpClient
    {
        private readonly ISerializationOption _serializationOption;
        private string _url, _token;

        public HttpClient(string url, ISerializationOption serializationOption, [Optional] string token)
        {
            _url = url;
            _serializationOption = serializationOption;
            _token = token;
        }

        public async Task<TModel> Get<TModel>(string endpoint, [Optional] Action<bool, float> status) =>
            await Request<TModel>(
                UnityWebRequest.Get(_url + endpoint),
                status: (isDone, downloadProgress) => status(isDone, downloadProgress)
            );

        public async Task<TModel> Post<TModel>(string endpoint, WWWForm form, [Optional] Action<bool, float> status) =>
            await Request<TModel>(
                UnityWebRequest.Post(_url + endpoint, form),
                status: (isDone, downloadProgress) => status(isDone, downloadProgress)
            );

        public async Task<JSONNode> Get(string endpoint, [Optional] Action<bool, float> status) =>
            await Request(
                UnityWebRequest.Get(_url + endpoint),
                status: (isDone, downloadProgress) => status(isDone, downloadProgress)
            );

        public async Task<JSONNode> Post(string endpoint, WWWForm form, [Optional] Action<bool, float> status) =>
            await Request(
                UnityWebRequest.Post(_url + endpoint, form),
                status: (isDone, downloadProgress) => status(isDone, downloadProgress)
            );

        private async Task<TModel> Request<TModel>(UnityWebRequest req, [Optional] Action<bool, float> status)
        {
            req.SetRequestHeader("Content-Type", _serializationOption.ContentType);

            if (_token != null) req.SetRequestHeader("Authorization", _token);

            var operation = req.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
                status(operation.isDone, req.downloadProgress);
            }

            if (req.result != UnityWebRequest.Result.Success) Debug.LogError($"Failed: {req.error}");
            var resDebug = JsonConvert.DeserializeObject<MPlayerInfo>(req.downloadHandler.text);

            var res = JsonConvert.DeserializeObject<TModel>(req.downloadHandler.text);

            Debug.Log("response : " + resDebug.data.uname);

            return res;
        }

        private async Task<JSONNode> Request(UnityWebRequest www, [Optional] Action<bool, float> status)
        {
            www.SetRequestHeader("Content-Type", _serializationOption.ContentType);

            if (_token != null) www.SetRequestHeader("Authorization", _token);

            var operation = www.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();

                status(
                    operation.isDone,
                    www.downloadProgress
                );
            }

            if (www.result != UnityWebRequest.Result.Success) Debug.LogError($"Failed: {www.error}");

            return _serializationOption.Deserialize<JSONNode>(www.downloadHandler.text)["data"];
        }

    }
}
