using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using RunMinigames.Interface;

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

        public async Task<TModel> Get<TModel>(string endpoint, [Optional] Action<bool, bool, float> status) =>
            await Request<TModel>(
                UnityWebRequest.Get(_url + endpoint),
                status: (isSuccess, isLoading, downloadProgress) => status(isSuccess, isLoading, downloadProgress)
            );

        public async Task<TModel> Post<TModel>(string endpoint, WWWForm form, [Optional] Action<bool, bool, float> status) =>
            await Request<TModel>(
                UnityWebRequest.Post(_url + endpoint, form),
                status: (isSuccess, isLoading, downloadProgress) => status(isSuccess, isLoading, downloadProgress)
            );

        private async Task<TModel> Request<TModel>(UnityWebRequest www, [Optional] Action<bool, bool, float> status)
        {
            www.SetRequestHeader("Content-Type", _serializationOption.ContentType);

            if (_token != null) www.SetRequestHeader("Authorization", _token);

            var operation = www.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();

                status(
                    www.result == UnityWebRequest.Result.Success,
                    !operation.isDone,
                    www.downloadProgress
                );
            }

            if (www.result != UnityWebRequest.Result.Success) Debug.LogError($"Failed: {www.error}");

            return _serializationOption.Deserialize<TModel>(www.downloadHandler.text);
        }

    }
}
