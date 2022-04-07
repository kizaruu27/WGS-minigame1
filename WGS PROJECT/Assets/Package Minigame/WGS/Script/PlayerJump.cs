using UnityEngine;
using UnityEngine.UI;


public class PlayerJump : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] Button btnJump;
    public bool isGrounded;
    public bool isJumping;
    WGS_PlayerRun playerMove;

    public Animator PlayerAnim;

    private void Awake()
    {
        btnJump.gameObject.SetActive(CheckPlatform.Mine.isAndroid || CheckPlatform.Mine.isIos);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        playerMove = GetComponent<WGS_PlayerRun>();

        //hapus platform unity kalau ingin production build
        if (CheckPlatform.Mine.isMacUnity || CheckPlatform.Mine.isWindowsUnity || CheckPlatform.Mine.isWeb)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && playerMove.CanMove)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                PlayerAnim.SetTrigger("Jump");
                isGrounded = false;
            }
        }

    }

    private void OnEnable()
    {
        if (CheckPlatform.Mine.isIos || CheckPlatform.Mine.isAndroid)
            btnJump.onClick.AddListener(ButtonClick);
    }

    public void ButtonClick()
    {
        if (isGrounded && playerMove.CanMove)
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


}
