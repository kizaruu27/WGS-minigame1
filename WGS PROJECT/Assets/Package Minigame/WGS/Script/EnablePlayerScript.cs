using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlayerScript : MonoBehaviour
{
    WGS_PlayerRun run;
    PlayerJump jump;

    // Start is called before the first frame update
    void Awake()
    {
        run = GetComponent<WGS_PlayerRun>();
        jump = GetComponent<PlayerJump>();
    }

    // Update is called once per frame
    void Update()
    {
        run.enabled = true;
        jump.enabled = true;
    }
}
