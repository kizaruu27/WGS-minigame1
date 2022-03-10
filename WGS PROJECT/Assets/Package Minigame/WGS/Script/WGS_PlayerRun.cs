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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerSpeed += 1;
            TargetAnimator.Play(AnimRun);
        }

        if (PlayerSpeed >= 0)
        {
            PlayerSpeed -= 0.01f;
        } else
        {
            TargetAnimator.Play(AnimIdle);
        }

        if (PlayerSpeed >= 10 ){ // speed max = 10
            PlayerSpeed = 10;
        }

        Player.transform.position += new Vector3(0, 0, PlayerSpeed * Time.deltaTime);
    }
}
