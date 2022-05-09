using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Singleplayer_ItemSpeed : MonoBehaviour
{
    public static Singleplayer_ItemSpeed instance;

    [Header("Speed Value")]
    [SerializeField] float SpeedCharacter;
    [SerializeField] float SpeedTime;

    [Header("Speed UI")]
    [SerializeField] GameObject UITimer;
    [SerializeField] TextMeshProUGUI timer;

    string PlayerTag = "Player";
    string NPCTag = "NPC";

    float PrevPlayerSpeed = 0.01f;
    float PrevNPCSpeed = 0.01f;

    public bool speedUp;

    MeshRenderer mesh;

    private void Awake()
    {
        instance = this;
    }


    void Start() => mesh = GetComponent<MeshRenderer>();

    private void Update()
    {

        if (speedUp)
        {
            SpeedTime -= Time.deltaTime;
            timer.text = SpeedTime.ToString();
        }
        else if (!speedUp)
        {
            UITimer.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PlayerTag) StartCoroutine(UpSpeedPlayer(other));
        if (other.gameObject.tag == NPCTag) StartCoroutine(UpSpeedNPC(other));
    }

    IEnumerator UpSpeedPlayer(Collider collider)
    {
        UITimer.SetActive(true);

        mesh.enabled = false;
        GetComponent<SphereCollider>().enabled = false;

        WGS_PlayerRun PlayerMovement = collider.GetComponent<WGS_PlayerRun>();
        PlayerMovement.PlayerSpeed += SpeedCharacter;

        speedUp = true;

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
