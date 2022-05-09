using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Singleplayer_ItemStop : MonoBehaviour
{
    [Header("Stop Value")]
    public float TimeFreeze;
    string PlayerTag = "Player";
    string PlayerEnemy = "NPC";

    [Header("Stop UI")]
    [SerializeField] GameObject UITimer;
    [SerializeField] TextMeshProUGUI timer;

    MeshRenderer mesh;
    SphereCollider sphereCollider;

    bool playerStop;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        Destroy(gameObject, 60);

        if (playerStop)
        {
            TimeFreeze -= Time.deltaTime;
            timer.text = TimeFreeze.ToString();
        }
        else
        {
            UITimer.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == PlayerTag)
        {
            // print ("bisa bro");
            StartCoroutine(FreezeCanMove(collider));
        }
        if (collider.gameObject.tag == PlayerEnemy)
        {
            // print ("bisa bro");
            StartCoroutine(FreezeNPCCanMove(collider));
        }

    }
    IEnumerator FreezeCanMove(Collider collider)
    {
        UITimer.SetActive(true);
        playerStop = true;

        mesh.enabled = false;
        sphereCollider.enabled = false;

        WGS_PlayerRun PlayerMove = collider.GetComponent<WGS_PlayerRun>();
        PlayerMove.maxSpeed = 0;

        yield return new WaitForSeconds(TimeFreeze);

        PlayerMove.maxSpeed = 10;
        Destroy(gameObject);
    }
    IEnumerator FreezeNPCCanMove(Collider collider)
    {
        mesh.enabled = false;
        sphereCollider.enabled = false;

        WGS_NPCRun NPCPlayerMove = collider.GetComponent<WGS_NPCRun>();
        NPCPlayerMove.NPCCanMove = false;
        NPCPlayerMove.IsItemSpeedActive = false;

        yield return new WaitForSeconds(TimeFreeze);

        NPCPlayerMove.NPCCanMove = true;
        Destroy(gameObject);
    }
}
