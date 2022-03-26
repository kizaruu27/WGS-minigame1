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

    //player jump
    bool isGrounded;
    public float jumpForce;

    PhotonView view;

    [Header("Mobile")]
    [SerializeField] private bool isControlBtnActive = false;
    float screenWidth;
    Button btnJump;
    Button btnRun;

    private void Awake()
    {
        btnJump = GameObject.Find("BtnJump").GetComponent<Button>();
        btnRun = GameObject.Find("BtnRun").GetComponent<Button>();
    }

    void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

        screenWidth = Screen.width;

        btnJump.gameObject.SetActive((CheckPlatform.isAndroid || CheckPlatform.isIos) && isControlBtnActive);
        btnRun.gameObject.SetActive((CheckPlatform.isAndroid || CheckPlatform.isIos) && isControlBtnActive);
    }

    void Update()
    {

        if (view.IsMine)
        {
            StartCoroutine(Controler());

            if (PlayerSpeed >= 0)
            {
                PlayerSpeed -= 0.01f;
            }
            else if (PlayerSpeed >= 0)
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
                        Running(.1f);
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
        PlayerSpeed += runSpeed;
        TargetAnimator.SetBool("isRunning", true);
    }

    void Jumping()
    {
        if (isGrounded && view.IsMine)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }


    //player controller
    IEnumerator Controler()
    {
        yield return new WaitForSeconds(4);

        Desktop();
        Mobile();
    }

}
