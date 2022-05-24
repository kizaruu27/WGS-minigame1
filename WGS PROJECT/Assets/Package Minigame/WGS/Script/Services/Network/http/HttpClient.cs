using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using RunMinigames.Interface;

namespace RunMinigames.Services
{
    public class HttpClient
    {

        private readonly ISerializationOption _serializationOption;

        public bool isLoading { get; private set; }

        private string _url;
        private string _token;

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

        public async Task<TModel> Get<TModel>(string endpoint)
        {
            using var www = UnityWebRequest.Get(_url + endpoint);

            return await Request<TModel>(www);
        }

        public async Task<TModel> Post<TModel>(string endpoint, WWWForm form)
        {
            using var www = UnityWebRequest.Post(_url + endpoint, form);

            return await Request<TModel>(www);
        }


        private async Task<TModel> Request<TModel>(UnityWebRequest www)
        {
            try
            {
                www.SetRequestHeader("Content-Type", _serializationOption.ContentType);

                if (_token != null)
                    www.SetRequestHeader("Authorization", _token);

                var operation = www.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (www.result != UnityWebRequest.Result.Success)
                    Debug.LogError($"Failed: {www.error}");

                var result = _serializationOption.Deserialize<TModel>(www.downloadHandler.text);
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
