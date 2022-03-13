using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpeed : MonoBehaviour
{
    [SerializeField] float SpeedCharacter;
    [SerializeField] float SpeedTime;

    string PlayerTag = "Player";
    string NPCTag = "NPC";

    float PrevPlayerSpeed;
    float PrevNPCSpeed;


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
        WGS_PlayerRun PlayerMovement = collider.GetComponent<WGS_PlayerRun>();
        Debug.Log("Sebelum tambah speed : " + PlayerMovement.PlayerSpeed);

        PrevPlayerSpeed = PlayerMovement.PlayerSpeed;
        PlayerMovement.PlayerSpeed = SpeedCharacter;



        PlayerMovement.IsItemSpeedActivce = true;

        yield return new WaitForSeconds(SpeedTime);

        PlayerMovement.PlayerSpeed = PrevPlayerSpeed;
        PlayerMovement.IsItemSpeedActivce = false;
    }

    IEnumerator UpSpeedNPC(Collider collider)
    {
        mesh.enabled = false;
        WGS_NPCRun NPCMovement = collider.GetComponent<WGS_NPCRun>();

        PrevNPCSpeed = NPCMovement.PlayerSpeed;
        NPCMovement.PlayerSpeed = SpeedCharacter;
        NPCMovement.IsItemSpeedActivce = true;

        yield return new WaitForSeconds(SpeedTime);

        NPCMovement.IsItemSpeedActivce = false;
        NPCMovement.PlayerSpeed = PrevNPCSpeed;
    }


}
