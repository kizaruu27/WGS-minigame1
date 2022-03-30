using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpeed : MonoBehaviour
{
    [SerializeField] float SpeedCharacter;
    [SerializeField] float SpeedTime;

    string PlayerTag = "Player";
    string NPCTag = "NPC";
    string MultiplayerNPC = "Multiplayer_NPC";

    float PrevPlayerSpeed;
    float PrevNPCSpeed;


    MeshRenderer mesh;


    void Start() => mesh = GetComponent<MeshRenderer>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PlayerTag) StartCoroutine(UpSpeedPlayer(other));
        if (other.gameObject.tag == NPCTag) StartCoroutine(UpSpeedNPC(other));
        if (other.gameObject.tag == MultiplayerNPC) StartCoroutine(UpSpeedNPC_Multiplayer(other));
    }

    IEnumerator UpSpeedPlayer(Collider collider)
    {
        mesh.enabled = false;
        PlayerRun_Multiplayer PlayerMovement = collider.GetComponent<PlayerRun_Multiplayer>();

        if (PlayerMovement.CanMove)
        {
            PrevPlayerSpeed = PlayerMovement.PlayerSpeed;
            PlayerMovement.PlayerSpeed += SpeedCharacter;
            PlayerMovement.IsItemSpeedActive = true;

            yield return new WaitForSeconds(SpeedTime);

            PlayerMovement.PlayerSpeed = PrevPlayerSpeed;
            PlayerMovement.IsItemSpeedActive = false;
            Destroy(gameObject);
        }

        PlayerMovement.IsItemSpeedActive = false;
        PlayerMovement.PlayerSpeed = 0;
        yield return new WaitForSeconds(0);
    }

    IEnumerator UpSpeedNPC(Collider collider)
    {
        mesh.enabled = false;
        WGS_NPCRun NPCMovement = collider.GetComponent<WGS_NPCRun>();

        if (NPCMovement.NPCCanMove)
        {
            PrevNPCSpeed = NPCMovement.PlayerSpeed;
            NPCMovement.PlayerSpeed = SpeedCharacter;

            NPCMovement.IsItemSpeedActive = true;


            yield return new WaitForSeconds(SpeedTime);

            NPCMovement.IsItemSpeedActive = false;
            NPCMovement.PlayerSpeed = PrevNPCSpeed;
            Destroy(gameObject);
        }

        NPCMovement.IsItemSpeedActive = false;

        yield return new WaitForSeconds(0);
    }

    IEnumerator UpSpeedNPC_Multiplayer(Collider collider)
    {
        mesh.enabled = false;
        Multiplayer_NPCRun NPCMovement = collider.GetComponent<Multiplayer_NPCRun>();

        if (NPCMovement.NPCCanMove)
        {
            PrevNPCSpeed = NPCMovement.PlayerSpeed;
            NPCMovement.PlayerSpeed = SpeedCharacter;

            NPCMovement.IsItemSpeedActive = true;


            yield return new WaitForSeconds(SpeedTime);

            NPCMovement.IsItemSpeedActive = false;
            NPCMovement.PlayerSpeed = PrevNPCSpeed;
            Destroy(gameObject);
        }

        NPCMovement.IsItemSpeedActive = false;

        yield return new WaitForSeconds(0);


    }


}
