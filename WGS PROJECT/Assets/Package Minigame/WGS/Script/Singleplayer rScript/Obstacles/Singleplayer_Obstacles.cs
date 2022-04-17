using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleplayer_Obstacles : MonoBehaviour
{
    [SerializeField] float newPlayerSpeed = 3f;
    [SerializeField] float delayTime = 3;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            StartCoroutine(playerSlowed(coll));
        }

        if (coll.gameObject.tag == "NPC")
        {
            //camera shake
            StartCoroutine(NPCSlowed(coll));
        }
    }

    IEnumerator playerSlowed(Collider coll)
    {
       WGS_PlayerRun playerMove = coll.GetComponent<WGS_PlayerRun>();
       playerMove.maxSpeed = newPlayerSpeed;

       yield return new WaitForSeconds(delayTime);

       playerMove.maxSpeed = 10;
    }

    IEnumerator NPCSlowed(Collider coll)
    {
        WGS_NPCRun NPCMove = coll.GetComponent<WGS_NPCRun>();
        NPCMove.MaxPlayerSpeed = newPlayerSpeed;

        yield return new WaitForSeconds(delayTime);

        NPCMove.MaxPlayerSpeed = 10;
    }
}
