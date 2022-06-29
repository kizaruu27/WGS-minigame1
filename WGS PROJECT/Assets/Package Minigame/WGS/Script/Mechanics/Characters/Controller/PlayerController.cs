using Photon.Pun;
using UnityEngine;

namespace RunMinigames.Mechanics.Characters
{
    public class PlayerController : CharactersController
    {
        public override void Jump()
        {
            if (IsGrounded && canMove)
            {
                Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                IsGrounded = false;
                //! TargetAnimator?.SetTrigger("Jump");
            }
        }

        private void OnCollisionEnter(Collision collider)
        {
            IsGrounded = collider.gameObject.tag == "Ground";
            //! TargetAnimator?.SetBool("isGrounded", IsGrounded);
        }
    }
}

