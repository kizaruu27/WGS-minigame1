using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using RunMinigames.Interface;
using M1_SimpleJSON;
using Newtonsoft.Json;
using RunMinigames.Models.Http.PlayerInfo;

namespace RunMinigames.Services.Http
{
  public class M1_HttpClient
  {
    private readonly M1_ISerializationOption _m1ISerializationOption;
    private string _url, _token;

    public M1_HttpClient(string url, M1_ISerializationOption m1ISerializationOption, [Optional] string token)
    {
      _url = url;
      _m1ISerializationOption = m1ISerializationOption;
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

    public async Task<M1_JSONNode> Get(string endpoint, [Optional] Action<bool, float> status) =>
        await Request(
            UnityWebRequest.Get(_url + endpoint),
            status: (isDone, downloadProgress) => status(isDone, downloadProgress)
        );

    public async Task<M1_JSONNode> Post(string endpoint, WWWForm form, [Optional] Action<bool, float> status) =>
        await Request(
            UnityWebRequest.Post(_url + endpoint, form),
            status: (isDone, downloadProgress) => status(isDone, downloadProgress)
        );

    private async Task<TModel> Request<TModel>(UnityWebRequest req, [Optional] Action<bool, float> status)
    {
      req.SetRequestHeader("Content-Type", _m1ISerializationOption.ContentType);

      if (_token != null) req.SetRequestHeader("Authorization", _token);

      var operation = req.SendWebRequest();

      while (!operation.isDone)
      {
        await Task.Yield();
        status(operation.isDone, req.downloadProgress);
      }

      if (req.result != UnityWebRequest.Result.Success) Debug.LogError($"Failed: {req.error}");
      var resDebug = JsonConvert.DeserializeObject<M1_MPlayerInfo>(req.downloadHandler.text);

      var res = JsonConvert.DeserializeObject<TModel>(req.downloadHandler.text);

      return res;
    }

    private async Task<M1_JSONNode> Request(UnityWebRequest www, [Optional] Action<bool, float> status)
    {
      www.SetRequestHeader("Content-Type", _m1ISerializationOption.ContentType);

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

      return _m1ISerializationOption.Deserialize<M1_JSONNode>(www.downloadHandler.text)["data"];
    }

  }
}
