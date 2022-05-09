using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Singleplayer_Obstacles : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] float newPlayerSpeed = 3f;
    [SerializeField] float delayTime = 3;

    [Header("Obstacle UI")]
    [SerializeField] GameObject UITimer;
    [SerializeField] TextMeshProUGUI timer;

    public bool hitObstacle;

    private void Update()
    {
        if (hitObstacle)
        {
            UITimer.SetActive(true);
            delayTime -= Time.deltaTime;
            timer.text = delayTime.ToString();

            if (delayTime <= 0)
            {
                UITimer.SetActive(false);
                hitObstacle = false;
            }
        }
        
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
             
            hitObstacle = true;
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
