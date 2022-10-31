using Photon.Pun;
using UnityEngine;

namespace RunMinigames.Mechanics.Characters
{
    public class M1_PlayerController : M1_ICharactersController
    {
        public override void Jump()
        {
            if (IsGrounded && canMove)
            {
                Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                IsGrounded = false;
                M1_PlayerSoundManager.instance.PlayJump();

            }
        }
    }
}

