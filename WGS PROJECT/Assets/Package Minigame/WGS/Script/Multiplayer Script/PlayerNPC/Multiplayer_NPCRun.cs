using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplayer_NPCRun : MonoBehaviour
{
    [Header("NPC AI Controler")]
    public GameObject Player;
    public Animator TargetAnimator;
    public string AnimIdle;
    public string AnimRun;
    public float MaxPlayerSpeed;
    public float PlayerSpeed;
    public bool NPCCanMove;
    public bool IsItemSpeedActive = false;

    private void Awake() => NPCCanMove = false;
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
        StartCoroutine(FreezeOnStart());
    }

    IEnumerator FreezeOnStart(){

        yield return new WaitForSeconds(3);
        NPCCanMove = true;
    }
}
