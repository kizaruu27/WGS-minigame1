using System;
using UnityEngine;
using RunMinigames.Interface;

namespace RunMinigames.Mechanics.Characters
{
    public abstract class CharactersController : MonoBehaviour, ICharacterItem
    {
        [Header("Base Character")]
        // public GameObject gameObject;
        public Animator TargetAnimator;
        protected Rigidbody Rb;


        [Header("Character Run")]
        [Range(0f, 10f)] public float maxSpeed = 10f;
        [Range(0f, 10f)] public float charSpeed;
        public bool canMove = true;
        public bool isItemSpeedActive = false;


        public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
        public float CharSpeed { get => charSpeed; set => charSpeed = value; }
        public bool CanMove { get => canMove; set => canMove = value; }
        public bool IsItemSpeedActive { get => isItemSpeedActive; set => isItemSpeedActive = value; }
        public bool Active { get => enabled; set => enabled = value; }
        
        [Header("Character Jump")]
        public float JumpForce = 6f;
        
        [Header("Ground Checker")]
        protected bool IsGrounded;
        [SerializeField] private Vector3 groundCheckOffset;
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private LayerMask whatIsGround;

        private void Awake()
        {
            TargetAnimator =
                GetComponentInChildren<Animator>(true) ??
                transform.Find("Humanoid").GetComponent<Animator>();
        }

        public virtual void Movement()
        {
            if (charSpeed >= 0 && !isItemSpeedActive)
            {
                if (!PlayerSoundManager.instance.audioSource.isPlaying)
                {
                    PlayerSoundManager.instance.PlayFootstepSound();
                }
                charSpeed -= 0.01f;
            }
            else if (charSpeed >= 0 && isItemSpeedActive)
            {
                TargetAnimator.SetBool("isRunning", true);
                
                if (!PlayerSoundManager.instance.audioSource.isPlaying)
                    PlayerSoundManager.instance.PlayFootstepSound();
            }
            else
            {
                TargetAnimator.SetBool("isRunning", false);
            }

            if (charSpeed >= maxSpeed)
            {
                charSpeed = maxSpeed;
            }

            gameObject.transform.position += new Vector3(0, 0, charSpeed * Time.deltaTime);
        }


        public virtual void Running(float runSpeed = 1f)
        {
            if (canMove)
            {
                TargetAnimator.SetBool("isRunning", true);

                if (!isItemSpeedActive)
                {
                    charSpeed += runSpeed;
                }
            }
            else
            {
                charSpeed = 0;
                TargetAnimator.SetBool("isRunning", false);
            }
        }

        public virtual void GroundCheck()
        {
            IsGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius,
                whatIsGround);

            if (!IsGrounded)
            {
                PlayerSoundManager.instance.StopSound();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
        }


        public abstract void Jump();
    }
}
