using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
   [SerializeField] float speed = 1.5f;
   Rigidbody rb;
   [SerializeField] bool isRight, isLeft;

   private void Awake() {
   }

   private void Start() {
       rb = GetComponent<Rigidbody>();
       isRight = true;
       isLeft = false;
       StartCoroutine(ItemMove());
   }

   private void FixedUpdate()
   {
        if (isRight && !isLeft) {
            rb.velocity = transform.right * speed;
            isLeft = false;
        }
        if (isLeft && !isRight) {
            rb.velocity = transform.forward * speed;
            isRight = false;
        }
   }

   private void OnTriggerEnter(Collider other)
   {
       if (other.gameObject.tag == "Player" || other.gameObject.tag == "NPC") {
           Destroy(gameObject);
       }
   }

   IEnumerator ItemMove() {
       yield return new WaitForSeconds (2);
       isLeft = true;
       isRight = false;

       yield return new WaitForSeconds (2);
       isLeft = false;
       isRight = true;

       StartCoroutine(ItemMove());
   }
}
