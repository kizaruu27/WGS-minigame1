
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

    private void Awake()
    {
        btnJump.gameObject.SetActive(CheckPlatform.isAndroid || CheckPlatform.isIos);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        playerMove = GetComponent<WGS_PlayerRun>();

        //hapus platform unity kalau ingin production build
        if (CheckPlatform.isMacUnity || CheckPlatform.isWindowsUnity || CheckPlatform.isWeb)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && playerMove.CanMove)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }

    }

    private void OnEnable()
    {
        if (CheckPlatform.isIos || CheckPlatform.isAndroid)
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
