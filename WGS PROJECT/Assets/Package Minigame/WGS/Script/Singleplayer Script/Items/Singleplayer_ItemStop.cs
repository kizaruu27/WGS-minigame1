using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleplayer_ItemStop : MonoBehaviour
{
    // [SerializeField] public float DisableCanMove;
    public float TimeFreeze;
    string PlayerTag = "Player";
    string PlayerEnemy = "NPC";

    MeshRenderer mesh;
    SphereCollider sphereCollider;


    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        Destroy(gameObject, 60);
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == PlayerTag)
        {
            // print ("bisa bro");
            mesh.enabled = false;
            sphereCollider.enabled = false;
            StartCoroutine(FreezeCanMove(collider));
        }
        if (collider.gameObject.tag == PlayerEnemy)
        {
            // print ("bisa bro");
            mesh.enabled = false;
            sphereCollider.enabled = false;
            StartCoroutine(FreezeNPCCanMove(collider));
        }

    }
    IEnumerator FreezeCanMove(Collider collider)
    {
        WGS_PlayerRun PlayerMove = collider.GetComponent<WGS_PlayerRun>();
        PlayerMove.CanMove = false;
        PlayerMove.IsItemSpeedActive = false;
        PlayerMove.PlayerSpeed = 0;

        yield return new WaitForSeconds(TimeFreeze);

        PlayerMove.CanMove = true;

        Destroy(gameObject);
    }
    IEnumerator FreezeNPCCanMove(Collider collider)
    {
        WGS_NPCRun NPCPlayerMove = collider.GetComponent<WGS_NPCRun>();
        NPCPlayerMove.NPCCanMove = false;
        NPCPlayerMove.IsItemSpeedActive = false;

        yield return new WaitForSeconds(TimeFreeze);

        NPCPlayerMove.NPCCanMove = true;
        Destroy(gameObject);
    }
}
