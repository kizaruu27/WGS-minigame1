using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using RunMinigames.Interface;

namespace RunMinigames.Services
{
    public class HttpClient
    {
#nullable enable

        private readonly ISerializationOption _serializationOption;

        public bool isLoading { get; private set; }

        private string _url;
        private string? _token;

        public HttpClient(string url, ISerializationOption serializationOption, string? token)
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

        public async Task<T> Get<T>(string endpoint)
        {
            try
            {
                using var www = UnityWebRequest.Get(_url + endpoint);
                www.SetRequestHeader("Content-Type", "application/json");

                if (_token != null)
                    www.SetRequestHeader("Authorization", _token);

                var operation = www.SendWebRequest();

                isLoading = false;

                while (!operation.isDone)
                {
                    isLoading = true;
                    await Task.Yield();
                }

                if (www.result != UnityWebRequest.Result.Success)
                    Debug.LogError($"Failed: {www.error}");

                var result = _serializationOption.Deserialize<T>(www.downloadHandler.text);

                if (result != null) isLoading = false;

                return result;
            }
            catch (Exception error)
            {
#nullable disable
                if (error != null) isLoading = false;

                Debug.LogError($"{nameof(Get)} failed: {error.Message}");
                return default;
            }
        }

        public async Task<T> Post<T>(string endpoint, WWWForm form)
        {
            try
            {
                using var www = UnityWebRequest.Post(_url + endpoint, form);
                www.SetRequestHeader("Content-Type", "application/json");

                if (_token != null)
                    www.SetRequestHeader("Authorization", _token);

                var operation = www.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (www.result != UnityWebRequest.Result.Success)
                    Debug.LogError($"Failed: {www.error}");

                var result = JsonUtility.FromJson<T>(www.downloadHandler.text);

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
