using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlayerSpeed += 1f;
                TargetAnimator.SetBool("isRunning", true);
            }

            if (PlayerSpeed >= 0)
            {
                PlayerSpeed -= 0.01f;
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

        Jump();
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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
