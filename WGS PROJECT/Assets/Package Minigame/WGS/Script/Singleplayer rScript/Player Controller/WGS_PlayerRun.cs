using UnityEngine;
using UnityEngine.UI;

public class WGS_PlayerRun : MonoBehaviour
{
    public static WGS_PlayerRun player;

    [Header("Player")]
    Rigidbody rb;
    public GameObject Player;
    public Animator TargetAnimator;

    [Header("Player Speed")]
    public float PlayerSpeed;
    public float maxSpeed;

    [Header("Player Jump")]
    public float jumpForce;
    bool isGrounded;

    [Header("Button Control")]
    public Button btnRun;
    public Button btnJump;

    private void Awake()
    {
        btnRun.gameObject.SetActive(CheckPlatform.isAndroid || CheckPlatform.isIos);
        btnJump.gameObject.SetActive(CheckPlatform.isAndroid || CheckPlatform.isIos);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (CheckPlatform.isMacUnity || CheckPlatform.isWindowsUnity || CheckPlatform.isWeb)
        {
            //player input
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Run();
            }

            //player jump
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }

        //player speed handler
        if (PlayerSpeed >= 0)
        {
            PlayerSpeed -= 0.01f;
        }
        else
        {
            TargetAnimator.SetBool("isRunning", false);
        }

        if (maxSpeed == 0)
        {
            TargetAnimator.SetBool("isRunning", false);
        }

        if (PlayerSpeed >= maxSpeed)
        {
            PlayerSpeed = maxSpeed;
        }

        Player.transform.position += new Vector3(0, 0, PlayerSpeed * Time.deltaTime);
    }
    private void OnEnable()
    {
        btnRun.onClick.AddListener(Run);
        btnJump.onClick.AddListener(Jump);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            TargetAnimator.SetBool("isGrounded", isGrounded);
        }
    }

    void Run()
    {
        PlayerSpeed += 1;
        TargetAnimator.SetBool("isRunning", true);
    }
    void Jump()
    {
        if (isGrounded)
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        isGrounded = false;
        TargetAnimator.SetTrigger("Jump");
    }

}
