using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using RunMinigames.Interface;

namespace RunMinigames.Services.Http
{
    public class HttpClient
    {

        private readonly ISerializationOption _serializationOption;

        public bool isLoading { get; private set; }
        public bool isSuccess { get; private set; }

        private string _url, _token;

        public HttpClient(string url, ISerializationOption serializationOption, string token)
        {
            _url = url;
            _serializationOption = serializationOption;
            _token = token;
        }

        public HttpClient(string url, ISerializationOption serializationOption)
        {
            _url = url;
            _serializationOption = serializationOption;
        }

        public async Task<TModel> Get<TModel>(string endpoint) =>
            await Request<TModel>(UnityWebRequest.Get(_url + endpoint));

        public async Task<TModel> Post<TModel>(string endpoint, WWWForm form) =>
            await Request<TModel>(UnityWebRequest.Post(_url + endpoint, form));

        private async Task<TModel> Request<TModel>(UnityWebRequest www)
        {
            try
            {
                www.SetRequestHeader("Content-Type", _serializationOption.ContentType);

                if (_token != null)
                    www.SetRequestHeader("Authorization", _token);

                var operation = www.SendWebRequest();

                isLoading = operation.isDone;

                while (!operation.isDone)
                {
                    await Task.Yield();
                }


                if (www.result != UnityWebRequest.Result.Success)
                    Debug.LogError($"Failed: {www.error}");

                var result = _serializationOption.Deserialize<TModel>(www.downloadHandler.text);

                isSuccess = www.result == UnityWebRequest.Result.Success;

                return result;
            }
            catch (Exception error)
            {
                Debug.LogError($"{nameof(Post)} failed: {error.Message}");
                return default;
            }
        }

    }
}
