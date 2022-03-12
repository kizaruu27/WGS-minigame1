using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
   [SerializeField] float speed = 1.5f;
   Rigidbody rb;
   [SerializeField] bool isRight, isLeft;

   private void Awake() {
       rb = GetComponent<Rigidbody>();
   }

   private void Start() {
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
