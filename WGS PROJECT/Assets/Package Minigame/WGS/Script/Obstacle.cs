using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float newPlayerSpeed;
    [SerializeField] float delayTime;


    CheckGameType type;
    PhotonView view;


    ItemCountdown itemCountdown;
    GameObject itemCountdownGO;

    private void Awake()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        type = gameManager.GetComponent<CheckGameType>();


        itemCountdown = Resources.FindObjectsOfTypeAll<ItemCountdown>()[0];
        itemCountdownGO = itemCountdown.gameObject;
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            view = coll.gameObject?.GetComponent<PhotonView>();

            Zetcode_CameraFollowPlayerFixed.cameraFollow.isShake = (type.IsMultiplayer && view.IsMine) || type.IsSingleplayer;

            StartCoroutine(playerSlowed(coll));
        }

        if (coll.gameObject.tag == "NPC")
        {
            //camera shake
            StartCoroutine(NPCSlowed(coll));
        }

        if (coll.gameObject.tag == "Multiplayer_NPC")
        {
            //camera shake
            StartCoroutine(Multiplayer_NPCSlowed(coll));
        }

    }

    IEnumerator playerSlowed(Collider coll)
    {
        PlayerRun_Multiplayer playerMove = coll.GetComponent<PlayerRun_Multiplayer>();
        playerMove.maxSpeed = newPlayerSpeed;


        itemCountdownGO.SetActive(true);
        itemCountdown.isItemSpeed = false;
        itemCountdown.enabled = true;
        itemCountdown.time = delayTime;

        yield return new WaitForSeconds(delayTime);

        itemCountdownGO.SetActive(false);
        itemCountdown.time = 0f;

        playerMove.maxSpeed = 10;
    }

    IEnumerator NPCSlowed(Collider coll)
    {
        WGS_NPCRun NPCMove = coll.GetComponent<WGS_NPCRun>();
        NPCMove.MaxPlayerSpeed = newPlayerSpeed;

        yield return new WaitForSeconds(delayTime);

        NPCMove.MaxPlayerSpeed = 10;
    }

    IEnumerator Multiplayer_NPCSlowed(Collider coll)
    {
        Multiplayer_NPCRun Multiplayer_NPCMove = coll.GetComponent<Multiplayer_NPCRun>();
        Multiplayer_NPCMove.MaxPlayerSpeed = newPlayerSpeed;

        yield return new WaitForSeconds(delayTime);

        Multiplayer_NPCMove.MaxPlayerSpeed = 10;
    }
}
