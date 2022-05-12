using UnityEngine;

using UnityEngine.UI;
using RunMinigames.Mechanics.Characters;

//! Jangan Lupa tambah Multiplayer
namespace RunMinigames.Mechanics.Characters
{
    public class PlayerInput : PlayerController
    {
        [Header("Mobile")]
        [SerializeField] bool IsControlBtnActive = false;
        float screenWidth;
        Button btnJump;
        Button btnRun;

        private void Awake()
        {
            btnJump = FindInActiveObjectByName("BtnJump")?.GetComponent<Button>();
            btnRun = FindInActiveObjectByName("BtnRun")?.GetComponent<Button>();

            Rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            btnJump.gameObject.SetActive((CheckPlatform.isAndroid || CheckPlatform.isIos) && IsControlBtnActive);
            btnRun.gameObject.SetActive((CheckPlatform.isAndroid || CheckPlatform.isIos) && IsControlBtnActive);

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
            Desktop();
            Mobile();
            Movement();
        }

        private void OnEnable()
        {
            //Button Control
            if (CheckPlatform.isIos || CheckPlatform.isAndroid && IsControlBtnActive)
            {
                btnJump.onClick.AddListener(Jump);
                btnRun.onClick.AddListener(() => Running());
            }
        }

        void Desktop()
        {
            if (
                CheckPlatform.isMacUnity ||
                CheckPlatform.isWindowsUnity ||
                CheckPlatform.isWeb ||
                CheckPlatform.isMac ||
                CheckPlatform.isWindows
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
            if (
                CheckPlatform.isAndroid ||
                CheckPlatform.isIos ||
                CheckPlatform.isMobile ||
                CheckPlatform.isWeb
            )
            {
                //Touch Control
                if (!IsControlBtnActive)
                {

                    int i = 0;

                    while (i < UnityEngine.Input.touchCount)
                    {
                        if (UnityEngine.Input.GetTouch(i).position.x > screenWidth / 2)
                        {
                            if (canMove && !isItemSpeedActive)
                            {
                                Running(.07f);
                            }
                        }

                        if (UnityEngine.Input.GetTouch(i).position.x < screenWidth / 2)
                        {
                            Jump();
                        }

                        ++i;
                    }

                }
            }
        }
    }


}

