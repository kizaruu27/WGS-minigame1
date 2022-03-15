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

    void Start()
    {
        rb = Character.GetComponent<Rigidbody>();
        Collider = GetComponent<CapsuleCollider>();
        FeetDistance = Collider.bounds.extents.y;
    }

    void Update()
    {
        if (TypeCharacter == PLAYER)
        {
            IsJump = Input.GetKeyDown(KeyCode.Space) && IsGrounded;
        }

        if (TypeCharacter == NPC)
        {

        }
    }

    private void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.down);
        IsGrounded = Physics.Raycast(transform.position, fwd, FeetDistance + .1f);

        if (IsJump) rb.velocity = new Vector3(0, JumpHeight, 0);
    }
}
