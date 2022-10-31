using UnityEngine;

namespace RunMinigames.Mechanics.Characters
{
    public class M1_NPCController : M1_ICharactersController
    {
        CapsuleCollider Collider;
        float FeetDistance;
        [SerializeField] bool IsJump;

        private void Start()
        {
            Collider = gameObject.GetComponent<CapsuleCollider>();
            FeetDistance = Collider.bounds.extents.y;

            Rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Jump();
            Running(0.01f);
            Movement();
        }

        private void FixedUpdate()
        {
            Vector3 fwd = transform.TransformDirection(Vector3.down);
            IsGrounded = Physics.Raycast(transform.position, fwd, FeetDistance + .1f);

            if (IsJump && canMove) Rb.velocity = new Vector3(0, JumpForce, 0);
        }

        public override void Jump()
        {
            Ray ray = new Ray();
            RaycastHit hit;
            ray.origin = gameObject.transform.position + (transform.forward * 1);
            ray.direction = Vector3.forward;

            IsJump = Physics.Raycast(ray, out hit, 1f);
        }

        public override void Movement()
        {
            if (charSpeed >= maxSpeed && !isItemSpeedActive)
            {
                charSpeed = maxSpeed;
            }

            gameObject.transform.position += new Vector3(0, 0, charSpeed * Time.deltaTime);
        }

        public override void Running(float runSpeed = 1) => base.Running(runSpeed);
    }
}
