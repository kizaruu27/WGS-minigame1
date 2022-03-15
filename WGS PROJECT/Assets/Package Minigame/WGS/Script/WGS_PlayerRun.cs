using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGS_PlayerRun : MonoBehaviour
{

    public GameObject Player;
    public Animator TargetAnimator;
    public string AnimIdle;
    public string AnimRun;
    public float PlayerSpeed;
    public bool CanMove;
    public float JumpHeight = 3f;
    public float GravityValue = -9.81f;

    public bool IsItemSpeedActive = false;

    CapsuleCollider Collider;

    float FeetDistance;

    Rigidbody rb;
    bool IsGrounded = true;
    bool IsJump;

    // Start is called before the first frame update
    void Start()
    {
        rb = Player.GetComponent<Rigidbody>();
        Collider = GetComponent<CapsuleCollider>();
        FeetDistance = Collider.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.position.y);

        IsJump = Input.GetKeyDown(KeyCode.Space) && IsGrounded;

        if (CanMove)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !IsItemSpeedActive)
            {
                PlayerSpeed += 1f;
                TargetAnimator.Play(AnimRun);
            }

        }
        else
        {
            PlayerSpeed = 0;
            TargetAnimator.Play(AnimIdle);
        }

        if (PlayerSpeed >= 0 && !IsItemSpeedActive) PlayerSpeed -= 0.01f;
        else if (PlayerSpeed >= 0 && IsItemSpeedActive) TargetAnimator.Play(AnimRun);
        else TargetAnimator.Play(AnimIdle);

        if (PlayerSpeed >= 10) PlayerSpeed = 10;


        Player.transform.position += new Vector3(0, 0, PlayerSpeed * Time.deltaTime);
    }


    private void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.down);
        IsGrounded = Physics.Raycast(transform.position, fwd, FeetDistance + .1f);

        if (IsJump) rb.velocity = new Vector3(0, JumpHeight, 0);
    }
}
