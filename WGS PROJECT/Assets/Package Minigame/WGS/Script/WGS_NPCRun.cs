using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGS_NPCRun : MonoBehaviour
{

    public GameObject Player;
    public Animator TargetAnimator;
    public string AnimIdle;
    public string AnimRun;
    public float MaxPlayerSpeed;
    public float PlayerSpeed;
    public bool NPCCanMove;
    public bool IsItemSpeedActive = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (NPCCanMove && !IsItemSpeedActive)
        {
            PlayerSpeed += 0.01f;
            TargetAnimator.SetBool("isRunning", true);
        }
        else if (IsItemSpeedActive) TargetAnimator.SetBool("isRunning", true);
        else
        {
            PlayerSpeed = 0;
            TargetAnimator.SetBool("isRunning", false);
        }

        if (PlayerSpeed >= MaxPlayerSpeed && !IsItemSpeedActive)
        {
            PlayerSpeed = MaxPlayerSpeed;
        }

        Player.transform.position += new Vector3(0, 0, PlayerSpeed * Time.deltaTime);
    }

}
