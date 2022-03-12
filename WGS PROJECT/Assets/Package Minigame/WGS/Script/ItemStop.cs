using System.Collections;
using UnityEngine;

public class ItemStop : MonoBehaviour
{
    // [SerializeField] public float DisableCanMove;
    public float TimeFreeze;
    string PlayerTag = "Player";
    string PlayerEnemy = "NPC";

    MeshRenderer mesh;


    private void Start() {
        mesh = GetComponent<MeshRenderer>();
    }


    private void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.tag == PlayerTag){
            // print ("bisa bro");
            mesh.enabled = false;
            StartCoroutine(FreezeCanMove(collider));
        }
        if(collider.gameObject.tag == PlayerEnemy){
            // print ("bisa bro");
            mesh.enabled = false;
            StartCoroutine(FreezeNPCCanMove(collider));
        }
        
    }
    IEnumerator FreezeCanMove(Collider collider){
            WGS_PlayerRun PlayerMove = collider.GetComponent<WGS_PlayerRun>();
            PlayerMove.CanMove = false;

            yield return new WaitForSeconds(TimeFreeze);
            
            //Destroy(gameObject);
            PlayerMove.CanMove = true;
    }
    IEnumerator FreezeNPCCanMove(Collider collider){
            WGS_NPCRun NPCPlayerMove = collider.GetComponent<WGS_NPCRun>();
            NPCPlayerMove.NPCCanMove = false;

            yield return new WaitForSeconds(TimeFreeze);
            
            //Destroy(gameObject);
            NPCPlayerMove.NPCCanMove = true;
    }
}
