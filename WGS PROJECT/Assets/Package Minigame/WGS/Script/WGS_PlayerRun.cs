using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGS_PlayerRun : MonoBehaviour
{

    [Header("Player")]
    public GameObject Player;
    public Animator TargetAnimator;

    [Header("Animation")]
    public string AnimIdle;
    public string AnimRun;

    [Header("Player Speed")]
    public float PlayerSpeed;
    public float maxSpeed;

    [Header("Items Validation")]
    public bool CanMove;
    public float JumpHeight = .1f;
    public bool IsItemSpeedActive = false;

    [Header("Player Jump")]
    public float GravityValue = -9.81f;
    float verticalVelocity = 0;
    bool IsGrounded;

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
