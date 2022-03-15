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
    public float maxSpeed;
    public bool CanMove;
    public float JumpHeight = .1f;
    public float GravityValue = -9.81f;

    public bool IsItemSpeedActive = false;

    float verticalVelocity = 0;
    bool IsGrounded;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(IsGrounded);
        if (IsGrounded)
        {

            if (CanMove)
            {
                Jump();

                if (Input.GetKeyDown(KeyCode.Mouse0) && !IsItemSpeedActive)
                {
                    PlayerSpeed += 1f;
                    TargetAnimator.Play(AnimRun);
                }

                if (IsItemSpeedActive) TargetAnimator.Play(AnimRun);

            }
            else
            {
                PlayerSpeed = 0;
                TargetAnimator.Play(AnimIdle);
            }

            if (PlayerSpeed >= 0 && !IsItemSpeedActive)
            {
                PlayerSpeed -= 0.01f;
            }
            else
            {
                TargetAnimator.Play(AnimIdle);
            }

            if (PlayerSpeed >= maxSpeed)
            { // speed max = 10
                PlayerSpeed = maxSpeed;
            }

            Player.transform.position += new Vector3(0, 0, PlayerSpeed * Time.deltaTime);
        }

    }


    private void OnCollisionStay(Collision other) => IsGrounded = true;


    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {

            verticalVelocity = Mathf.Sqrt(JumpHeight * -3.0f * GravityValue);
            IsGrounded = false;
        }
        else
        {
            verticalVelocity += GravityValue * Time.deltaTime;
            IsGrounded = true;
        }
    }


}
