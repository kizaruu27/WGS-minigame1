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
            //transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + 65, transform.rotation.z);
            isLeft = false;
        }
        if (isLeft && !isRight) {
            rb.velocity = transform.forward * speed;
            //transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y - 65, transform.rotation.z);
            isRight = false;
        }
   }

   private void OnTriggerEnter(Collider other)
   {
       if (other.gameObject.tag == "Player") {
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
