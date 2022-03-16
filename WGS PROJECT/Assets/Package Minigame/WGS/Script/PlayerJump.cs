using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float jumpForce = 5f;
    public bool isGrounded;
    public bool isJumping;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collider)
    {   
        if (collider.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
