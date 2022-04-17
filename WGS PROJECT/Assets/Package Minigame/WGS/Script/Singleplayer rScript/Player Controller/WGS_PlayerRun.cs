using UnityEngine;

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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //player input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerSpeed += 1;
            TargetAnimator.SetBool("isRunning", true);
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

        //player jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            TargetAnimator.SetBool("isGrounded", isGrounded);
        }
    }

    void Jump()
    {
        rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        isGrounded = false;
        TargetAnimator.SetTrigger("Jump");
    }

}
