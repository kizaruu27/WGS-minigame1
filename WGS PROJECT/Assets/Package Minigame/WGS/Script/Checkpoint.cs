using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isFinishLine = false;
    public bool stopAfterFinish = false;
    public int checkPointNumber = 1;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && stopAfterFinish == true)
        {
            WGS_PlayerRun PlayerMove = collider.GetComponent<WGS_PlayerRun>();
            PlayerMove.maxSpeed = 0;
        }
        if (collider.gameObject.tag == "NPC" && stopAfterFinish == true)
        {
            WGS_NPCRun NPCPlayerMove = collider.GetComponent<WGS_NPCRun>();
            NPCPlayerMove.NPCCanMove = false;
        }

    }
}
