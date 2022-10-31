using UnityEngine;

public class M1_CheckPlatform : MonoBehaviour
{
    public static RuntimePlatform app = Application.platform;

    //Platform
    public static bool isWindows { get => app == RuntimePlatform.WindowsPlayer; }
    public static bool isMac { get => app == RuntimePlatform.OSXPlayer; }
    public static bool isWeb { get => app == RuntimePlatform.WebGLPlayer; }
    public static bool isAndroid { get => app == RuntimePlatform.Android; }
    public static bool isIos { get => app == RuntimePlatform.IPhonePlayer; }

    //Platform Editor
    public static bool isMacUnity { get => app == RuntimePlatform.OSXEditor; }
    public static bool isWindowsUnity { get => app == RuntimePlatform.WindowsEditor; }


    //Device
    public static bool isDesktop { get => SystemInfo.deviceType == DeviceType.Desktop; }
    public static bool isMobile { get => SystemInfo.deviceType == DeviceType.Handheld; }


}
