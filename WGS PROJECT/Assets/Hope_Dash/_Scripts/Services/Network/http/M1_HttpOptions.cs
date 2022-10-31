using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

using UnityEngine;
using RunMinigames.Interface;
using M1_SimpleJSON;


namespace RunMinigames.Services.Http
{
    public class M1_HttpOptions : M1_ISerializationOption
    {
        public string ContentType => "application/json";

        [JsonConstructor]
        public M1_HttpOptions() { }

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


        public M1_JSONNode Deserialize(string text)
        {
            try
            {
                return M1_JSONNode.Parse(text);
            }
            catch (Exception error)
            {
                Debug.LogError($"Gagal Parse Response {text}. {error.Message}");
                return default;
            }
        }
    }
}