using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
   [SerializeField] float speed = 1.5f;

   private void Update()
   {
       transform.Translate(Vector3.forward * speed * Time.deltaTime);
   }

   private void OnTriggerEnter(Collider other)
   {
       if (other.gameObject.tag == "Player") {
           Destroy(gameObject);
       }
   }
}
