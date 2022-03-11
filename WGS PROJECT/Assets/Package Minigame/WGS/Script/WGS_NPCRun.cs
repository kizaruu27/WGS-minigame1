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
    float PlayerSpeed;
    public bool NPCCanMove;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (NPCCanMove){
            PlayerSpeed += 0.01f;
            TargetAnimator.Play(AnimRun);
        }else{
            PlayerSpeed = 0;
            TargetAnimator.Play(AnimIdle);
        }

        if (PlayerSpeed >= MaxPlayerSpeed)
        {
            PlayerSpeed = MaxPlayerSpeed;
        }
        Player.transform.position += new Vector3(0, 0, PlayerSpeed * Time.deltaTime);
    }
}
