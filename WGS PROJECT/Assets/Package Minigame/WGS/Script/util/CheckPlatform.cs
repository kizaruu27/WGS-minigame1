using UnityEngine;

public class CheckPlatform : MonoBehaviour
{

    public static CheckPlatform Mine;

    private void Awake() => Mine = this;

    public RuntimePlatform app = Application.platform;


    //Platform
    public bool isWindows { get => app == RuntimePlatform.WindowsPlayer; }
    public bool isMac { get => app == RuntimePlatform.OSXPlayer; }
    public bool isWeb { get => app == RuntimePlatform.WebGLPlayer; }
    public bool isAndroid { get => app == RuntimePlatform.Android; }
    public bool isIos { get => app == RuntimePlatform.IPhonePlayer; }

    //Platform Editor
    public bool isMacUnity { get => app == RuntimePlatform.OSXEditor; }
    public bool isWindowsUnity { get => app == RuntimePlatform.WindowsEditor; }


    //Device
    public bool isDesktop { get => SystemInfo.deviceType == DeviceType.Desktop; }
    public bool isMobile { get => SystemInfo.deviceType == DeviceType.Handheld; }


}
