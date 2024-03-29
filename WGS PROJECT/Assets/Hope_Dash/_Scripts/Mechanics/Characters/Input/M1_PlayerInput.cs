using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using RunMinigames.Mechanics.Characters;

//! Jangan Lupa tambah Multiplayer
namespace RunMinigames.Mechanics.Characters
{
  public class M1_PlayerInput : M1_PlayerController
  {
    [Header("Mobile")]
    [SerializeField] bool IsControlBtnActive = false;
    float screenWidth;
    Button btnJump;
    Button btnRun;
    public PhotonView view;


    private void Awake()
    {
      btnJump = FindInActiveObjectByName("BtnJump")?.GetComponent<Button>();
      btnRun = FindInActiveObjectByName("BtnRun")?.GetComponent<Button>();

      Rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
      // btnJump.gameObject.SetActive((CheckPlatform.isAndroid || CheckPlatform.isIos) && IsControlBtnActive);
      // btnRun.gameObject.SetActive((CheckPlatform.isAndroid || CheckPlatform.isIos) && IsControlBtnActive);

      screenWidth = Screen.width;
    }

    GameObject FindInActiveObjectByName(string name)
    {
      Button[] objs = Resources.FindObjectsOfTypeAll<Button>() as Button[];
      for (int i = 0; i < objs.Length; i++)
      {
        if (objs[i].hideFlags == HideFlags.None)
        {
          if (objs[i].name == name)
          {
            return objs[i].gameObject;
          }
        }
      }
      return null;
    }

    private void Update()
    {

      if (view.IsMine)
      {
        Desktop();
        Mobile();
      }

      Movement();
      GroundCheck();
    }

    private void OnEnable()
    {
      //Button Control
      // if (CheckPlatform.isIos || CheckPlatform.isAndroid && IsControlBtnActive && view.IsMine)
      // {
      //     btnJump.onClick.AddListener(Jump);
      //     btnRun.onClick.AddListener(() => Running());
      // }
    }

    void Desktop()
    {
      if (
          M1_CheckPlatform.isMacUnity ||
          M1_CheckPlatform.isWindowsUnity ||
          M1_CheckPlatform.isWeb ||
          M1_CheckPlatform.isMac ||
          M1_CheckPlatform.isWindows
      )
      {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
        {
          Running();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
          Jump();
        }
      }
    }

    void Mobile()
    {
      int i = 0;

      while (i < UnityEngine.Input.touchCount)
      {
        bool isTap = Input.GetTouch(i).phase == TouchPhase.Began;

        if (UnityEngine.Input.GetTouch(i).position.x > screenWidth / 2)
        {
          if (canMove && !isItemSpeedActive)
          {
            Running(.07f);
          }
        }

        if (UnityEngine.Input.GetTouch(i).position.x < screenWidth / 2 && isTap && IsGrounded)
        {
          Jump();
        }

        ++i;
      }
    }
  }
}