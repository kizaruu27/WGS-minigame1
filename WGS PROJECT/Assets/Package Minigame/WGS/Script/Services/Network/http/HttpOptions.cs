using Newtonsoft.Json;
using System;
using UnityEngine;
using RunMinigames.Interface;


namespace RunMinigames.Services.Http
{
    public class HttpOptions : ISerializationOption
    {
        public string ContentType => "application/json";

        public T Deserialize<T>(string text)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(text);
            }
            catch (Exception error)
            {
                Debug.LogError($"Gagal Parse Response {text}. {error.Message}");
                return default;
            }
        }
    }
}