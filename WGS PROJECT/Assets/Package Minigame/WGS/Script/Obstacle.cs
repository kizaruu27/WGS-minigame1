using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float newPlayerSpeed;
    [SerializeField] float delayTime;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Zetcode_CameraFollowPlayerFixed.cameraFollow.isShake = true;

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
        PlayerRun_Multiplayer playerMove = coll.GetComponent<PlayerRun_Multiplayer>();
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
