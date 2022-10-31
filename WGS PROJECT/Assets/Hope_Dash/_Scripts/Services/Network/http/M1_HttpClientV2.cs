using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using RunMinigames.Interface;
using M1_SimpleJSON;

namespace RunMinigames.Services.Http
{
    public class M1_HttpClientV2
    {
        private M1_ISerializationOption _m1ISerializationOption;
        private string _url, _token;

        public M1_HttpClientV2(string url, M1_ISerializationOption m1ISerializationOption, [Optional] string token)
        {
            _url = url;
            _m1ISerializationOption = m1ISerializationOption;
            _token = token;
        }

        public IEnumerator Get(string endpoint, Action<M1_JSONNode> res, [Optional] Action<float> reqProgress)
        {
            using var req = UnityWebRequest.Get(_url + endpoint);
            req.SetRequestHeader("Content-Type", _m1ISerializationOption.ContentType);
            if (_token != null) req.SetRequestHeader("Authorization", _token);

            yield return req.SendWebRequest();

            CheckRequest(req, res);
        }

        public IEnumerator Post(string endpoint, WWWForm form, Action<M1_JSONNode> res, [Optional] Action<float> reqProgress)
        {
            using var req = UnityWebRequest.Post(_url + endpoint, form);
            req.SetRequestHeader("Content-Type", _m1ISerializationOption.ContentType);
            if (_token != null) req.SetRequestHeader("Authorization", _token);

            yield return req.SendWebRequest();

            CheckRequest(req, res);
        }

        void CheckRequest(UnityWebRequest req, Action<M1_JSONNode> res)
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
                    res?.Invoke(M1_JSON.Parse(req.downloadHandler.text));
                    break;
            }
        }
    }
}