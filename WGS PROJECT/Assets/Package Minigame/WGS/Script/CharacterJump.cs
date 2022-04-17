using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    [Header("Definition")]
    readonly string PLAYER = "Player";
    readonly string NPC = "NPC";


    [Header("Character")]
    [SerializeField] string TypeCharacter;
    [SerializeField] GameObject Character;
    Rigidbody rb;
    CapsuleCollider Collider;


    [Header("Jump")]
    [SerializeField] float JumpHeight = 3f;
    float FeetDistance;
    bool IsGrounded = true;
    bool IsJump;
    bool IsMove;

    void Start()
    {
        rb = Character.GetComponent<Rigidbody>();
        Collider = Character.GetComponent<CapsuleCollider>();
        FeetDistance = Collider.bounds.extents.y;
    }

    void Update()
    {

        if (TypeCharacter == PLAYER)
        {
            WGS_PlayerRun player = GetComponent<WGS_PlayerRun>();
            //IsMove = player.CanMove;

            IsJump = Input.GetKeyDown(KeyCode.Space) && IsGrounded;
        }

        if (TypeCharacter == NPC)
        {
            WGS_NPCRun npc = GetComponent<WGS_NPCRun>();
            IsMove = npc.NPCCanMove;

            Ray ray = new Ray();
            RaycastHit hit;
            ray.origin = Character.transform.position + (transform.forward * 1);
            ray.direction = Vector3.forward;

            IsJump = Physics.Raycast(ray, out hit, 2f);
        }
    }

    private void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.down);
        IsGrounded = Physics.Raycast(transform.position, fwd, FeetDistance + .1f);

        if (IsJump && IsMove) rb.velocity = new Vector3(0, JumpHeight, 0);
    }
}
