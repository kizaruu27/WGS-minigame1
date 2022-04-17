using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleplayer_ItemSpeed : MonoBehaviour
{
    [SerializeField] float SpeedCharacter;
    [SerializeField] float SpeedTime;

    string PlayerTag = "Player";
    string NPCTag = "NPC";

    float PrevPlayerSpeed = 0.01f;
    float PrevNPCSpeed = 0.01f;

    MeshRenderer mesh;


    void Start() => mesh = GetComponent<MeshRenderer>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PlayerTag) StartCoroutine(UpSpeedPlayer(other));
        if (other.gameObject.tag == NPCTag) StartCoroutine(UpSpeedNPC(other));
    }

    IEnumerator UpSpeedPlayer(Collider collider)
    {
        mesh.enabled = false;
        GetComponent<SphereCollider>().enabled = false;

        WGS_PlayerRun PlayerMovement = collider.GetComponent<WGS_PlayerRun>();
        PlayerMovement.PlayerSpeed += SpeedCharacter;

        yield return new WaitForSeconds(SpeedTime);

        PlayerMovement.PlayerSpeed -= PrevPlayerSpeed;
        Destroy(gameObject);
    }

    IEnumerator UpSpeedNPC(Collider collider)
    {
        mesh.enabled = false;
        GetComponent<SphereCollider>().enabled = false;

        WGS_NPCRun NPC = collider.GetComponent<WGS_NPCRun>();
        NPC.PlayerSpeed += SpeedCharacter;

        yield return new WaitForSeconds(SpeedTime);

        NPC.PlayerSpeed -= PrevNPCSpeed;
        Destroy(gameObject);
    }


}
