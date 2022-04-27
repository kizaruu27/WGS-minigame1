using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;


public class PlayerRun_Multiplayer : MonoBehaviour
{
    [Header("Player")]
    Rigidbody rb;
    public GameObject Player;
    public Animator TargetAnimator;

    [Header("Animation")]
    public string AnimIdle;
    public string AnimRun;

    [Header("Player Speed")]
    public float PlayerSpeed;
    public float maxSpeed;
    public bool CanMove = true;
    public bool IsItemSpeedActive = false;

    //player jump
    bool isGrounded;
    public float jumpForce;

    PhotonView view;

    [Header("Mobile")]
    [SerializeField] private bool isControlBtnActive = false;
    float screenWidth;
    Button btnJump;
    Button btnRun;
    bool isGameStart;

    private void Awake()
    {
        btnJump = FindInActiveObjectByName("BtnJump")?.GetComponent<Button>();
        btnRun = FindInActiveObjectByName("BtnRun")?.GetComponent<Button>();
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

    void Start()
    {
        btnJump.gameObject.SetActive((CheckPlatform.isAndroid || CheckPlatform.isIos) && isControlBtnActive);
        btnRun.gameObject.SetActive((CheckPlatform.isAndroid || CheckPlatform.isIos) && isControlBtnActive);

        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

        screenWidth = Screen.width;

    }

    void Update()
    {
        if (view.IsMine)
        {
            StartCoroutine(
                CheckAllPlayerConnected.instance.WaitAllPlayerReady(
                    () => StartCoroutine(
                        Controler()
                    )
                )
            );

            if (PlayerSpeed >= 0 && !IsItemSpeedActive)
            {
                PlayerSpeed -= 0.01f;
            }
            else if (PlayerSpeed >= 0 && IsItemSpeedActive)
            {
                TargetAnimator.SetBool("isRunning", true);
            }
            else
            {
                TargetAnimator.SetBool("isRunning", false);
            }

            if (PlayerSpeed >= maxSpeed)
            {
                PlayerSpeed = maxSpeed;
            }

            Player.transform.position += new Vector3(0, 0, PlayerSpeed * Time.deltaTime);
        }


    }

    private void OnEnable()
    {
        Debug.Log(CheckPlatform.isIos || CheckPlatform.isAndroid && isControlBtnActive & isGameStart);
        if (CheckPlatform.isIos || CheckPlatform.isAndroid && isControlBtnActive)
        {
            btnJump.onClick.AddListener(Jumping);
            btnRun.onClick.AddListener(() => Running());
        }
    }

    //device
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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Running();

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jumping();
            }
        }
    }

    void Mobile()
    {
        if (
             CheckPlatform.isAndroid ||
             CheckPlatform.isIos ||
             CheckPlatform.isMobile
        )
        {
            if (!isControlBtnActive)
            {

                int i = 0;

                while (i < Input.touchCount)
                {
                    if (Input.GetTouch(i).position.x > screenWidth / 2)
                    {
                        if (CanMove && !IsItemSpeedActive)
                        {
                            Running(.07f);
                        }
                    }

                    if (Input.GetTouch(i).position.x < screenWidth / 2)
                    {
                        Jumping();
                    }

                    ++i;
                }

            }
        }
    }


    //Control
    void Running(float runSpeed = 1f)
    {
        if (CanMove && isGameStart)
        {
            if (!IsItemSpeedActive)
            {
                PlayerSpeed += runSpeed;
                TargetAnimator.SetBool("isRunning", true);
            }
        }
        else
        {
            PlayerSpeed = 0;
            TargetAnimator.SetBool("isRunning", false);
        }
    }

    void Jumping()
    {
        if (isGrounded && view.IsMine && CanMove && isGameStart)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            TargetAnimator.SetTrigger("Jump");
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        isGrounded = collider.gameObject.tag == "Ground";
        TargetAnimator.SetBool("isGrounded", isGrounded);
    }


    IEnumerator Controler()
    {
        yield return new WaitForSeconds(4);
        isGameStart = true;
        Desktop();
        Mobile();
    }

}
