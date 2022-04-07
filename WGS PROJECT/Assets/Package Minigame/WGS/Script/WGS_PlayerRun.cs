using UnityEngine.UI;
using UnityEngine;

public class WGS_PlayerRun : MonoBehaviour
{
    public static WGS_PlayerRun player;

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

    [Header("Items Validation")]
    public bool CanMove;
    public bool IsItemSpeedActive = false;

    [Header("Jump")]
    public float jumpForce = 5f;
    bool isJumping;

    [Header("Mobile Button Run")]
    public Button btnRun;


    private void Awake()
    {
        player = this;

        btnRun.gameObject.SetActive(CheckPlatform.Mine.isAndroid || CheckPlatform.Mine.isIos);
    }

    void Update()
    {
        if (CheckPlatform.Mine.isMacUnity || CheckPlatform.Mine.isWindowsUnity || CheckPlatform.Mine.isWeb)
        {
            if (CanMove)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && !IsItemSpeedActive && !FinishChecker.finishChecker.isFinish)
                {
                    PlayerSpeed += 1f;
                    TargetAnimator.SetBool("isRunning", true);
                }
            }
            else
            {
                PlayerSpeed = 0;
                TargetAnimator.SetBool("isRunning", false);
            }
        }

        if (PlayerSpeed >= 0 && !IsItemSpeedActive)
        {
            PlayerSpeed -= 0.01f;
        }
        else if (PlayerSpeed >= 0 && IsItemSpeedActive)
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

    private void OnEnable()
    {
        if (CheckPlatform.Mine.isIos || CheckPlatform.Mine.isAndroid)
            btnRun.onClick.AddListener(MobileBtnRun);
    }

    public void MobileBtnRun()
    {

        if (CanMove)
        {
            if (!IsItemSpeedActive && !FinishChecker.finishChecker.isFinish)
            {
                PlayerSpeed += 1f;
                TargetAnimator.SetBool("isRunning", true);
            }
        }
        else
        {
            PlayerSpeed = 0;
            TargetAnimator.SetBool("isRunning", false);
        }

    }
}
