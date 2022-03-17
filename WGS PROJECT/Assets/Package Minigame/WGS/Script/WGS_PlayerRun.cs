using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGS_PlayerRun : MonoBehaviour
{
    public static WGS_PlayerRun player;

    [Header("Player")]
    CapsuleCollider Collider;
    Rigidbody rb;
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
    public bool IsItemSpeedActive = false;

    [Header("Jump")]
    public float jumpForce = 5f;
    bool isJumping;

    private void Awake()
    {
        player = this;
    }

    void Update()
    {

        if (CanMove)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !IsItemSpeedActive && !FinishChecker.finishChecker.isFinish)
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
        else if (PlayerSpeed >= 0 && IsItemSpeedActive)
        {
            TargetAnimator.Play(AnimRun);
        }
        else
        {
            TargetAnimator.Play(AnimIdle);
        }


        if (PlayerSpeed >= maxSpeed)
        {
            PlayerSpeed = maxSpeed;
        }


        Player.transform.position += new Vector3(0, 0, PlayerSpeed * Time.deltaTime);
    }
}
