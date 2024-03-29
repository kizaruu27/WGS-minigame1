using UnityEngine;
using UnityEngine.SceneManagement;
using RunMinigames.Models.Http.PlayerInfo;
using RunMinigames.Services.Http;
using RunMinigames.Services.Photon;
using RunMinigames.View.Loading;
using M1_SimpleJSON;


namespace RunMinigames.Manager.Networking
{
  public class M1_HttpManager : MonoBehaviour
  {

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string GetToken();
#endif

    private bool deviceType;
    private M1_MPlayerInfo result;

    readonly string EditorToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjIiLCJpYXQiOjE2NTI4NTQ4NjEsImV4cCI6MTY4NDQxMTc4N30.WgPvma6Sn6bSgMcB09gCSmTB11np8RQG0ZLkBvB-AZ4";
    readonly string BuildToken = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjQiLCJpYXQiOjE2NTM2MjE0NzIsImV4cCI6MTY4NTE3ODM5OH0.PsKoprNpr3sudUxukyYA58d1Hx6amSWWIOj4YERBMGQ";
    string localToken;

    // authorization token for WebGL
    string authToken;
    string urlToken;
    M1_HttpClientV2 client;

    bool isStopRequest;
    M1_JSONNode data;
    Scene currScene;

    private void Awake()
    {
      GetToken();

      client = new M1_HttpClientV2(
                  M1_HttpConfig.BASE_URL,
                  new M1_HttpOptions(),
                  authToken
              );

      currScene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
      if (currScene.name == "M1_WGS1_Login" && !isStopRequest) GetUserData();
      // GetComponent<PhotonServer>().Connect("Play Test");
    }


    void GetToken()
    {

#if UNITY_WEBGL && !UNITY_EDITOR
            urlToken = "Bearer " + GetToken();
#endif
      string[] unityDataPath = Application.dataPath.Split('/');
      int unityPathLength = unityDataPath.Length - 2;
      int unityInstanceLength = unityDataPath[unityPathLength].Split('_').Length;

      Debug.Log(unityDataPath[unityPathLength]);

      localToken = unityInstanceLength > 1 ? EditorToken : BuildToken;
      deviceType = M1_CheckPlatform.isWeb && (!M1_CheckPlatform.isMacUnity || !M1_CheckPlatform.isWindowsUnity);
      authToken = deviceType ? urlToken : localToken;
    }


    void GetUserData()
    {
      M1_LoginStatus.instance.StepperMessage("Getting user data...");
      StartCoroutine(client.Get(M1_HttpConfig.ENDPOINT["user"], (res) =>
      {
        isStopRequest = res != null;
        M1_LoginStatus.instance.StepperMessage("process user data...");

        if (isStopRequest)
        {
          PlayerPrefs.SetString("token", authToken);
          PlayerPrefs.SetString("LocalPlayerData", res["data"].ToString());

          GetComponent<M1_PhotonServer>().Connect(M1_JSON.Parse(PlayerPrefs.GetString("LocalPlayerData"))["full_name"]);
        }
      }));
    }
  }
}