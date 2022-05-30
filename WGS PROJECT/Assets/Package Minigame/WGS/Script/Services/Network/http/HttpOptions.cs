using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

using UnityEngine;
using RunMinigames.Interface;
using SimpleJSON;


namespace RunMinigames.Services.Http
{
    public class HttpOptions : ISerializationOption
    {
        public string ContentType => "application/json";

        [JsonConstructor]
        public HttpOptions() { }

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


        public JSONNode Deserialize(string text)
        {
            try
            {
                return JSONNode.Parse(text);
            }
            catch (Exception error)
            {
                Debug.LogError($"Gagal Parse Response {text}. {error.Message}");
                return default;
            }
        }
    }
}