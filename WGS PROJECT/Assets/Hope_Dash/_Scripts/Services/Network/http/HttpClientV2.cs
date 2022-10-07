using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using RunMinigames.Interface;
using SimpleJSON;

namespace RunMinigames.Services.Http
{
    public class HttpClientV2
    {
        private ISerializationOption _serializationOption;
        private string _url, _token;

        public HttpClientV2(string url, ISerializationOption serializationOption, [Optional] string token)
        {
            _url = url;
            _serializationOption = serializationOption;
            _token = token;
        }

        public IEnumerator Get(string endpoint, Action<JSONNode> res, [Optional] Action<float> reqProgress)
        {
            using var req = UnityWebRequest.Get(_url + endpoint);
            req.SetRequestHeader("Content-Type", _serializationOption.ContentType);
            if (_token != null) req.SetRequestHeader("Authorization", _token);

            yield return req.SendWebRequest();

            CheckRequest(req, res);
        }

        public IEnumerator Post(string endpoint, WWWForm form, Action<JSONNode> res, [Optional] Action<float> reqProgress)
        {
            using var req = UnityWebRequest.Post(_url + endpoint, form);
            req.SetRequestHeader("Content-Type", _serializationOption.ContentType);
            if (_token != null) req.SetRequestHeader("Authorization", _token);

            yield return req.SendWebRequest();

            CheckRequest(req, res);
        }

        void CheckRequest(UnityWebRequest req, Action<JSONNode> res)
        {
            switch (req.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(" Error: " + req.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(" HTTP Error: " + req.error);
                    break;
                case UnityWebRequest.Result.Success:
                    res?.Invoke(JSON.Parse(req.downloadHandler.text));
                    break;
            }
        }
    }
}