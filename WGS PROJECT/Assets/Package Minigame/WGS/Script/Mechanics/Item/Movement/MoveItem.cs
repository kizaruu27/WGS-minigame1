using System.Collections;
using UnityEngine;


namespace RunMinigames.Mechanics.Items
{
    public class MoveItem : MonoBehaviour
    {
        [Header("Item Move")]
        [SerializeField] public float SpeedItem = 1.5f;
        [SerializeField] protected bool CanMove;

        Rigidbody rb;
        bool isRight, isLeft;

        private void Awake() => rb = gameObject?.GetComponent<Rigidbody>();

        private void Start()
        {
            if (CanMove)
            {
                isRight = true;
                isLeft = false;
                StartCoroutine(Move());
            }
        }

        private void FixedUpdate()
        {
            if (CanMove)
            {
                if (isRight && !isLeft)
                {
                    rb.velocity = transform.right * SpeedItem;
                    isLeft = false;
                }

                if (isLeft && !isRight)
                {
                    rb.velocity = transform.forward * SpeedItem;
                    isRight = false;
                }
            }
        }

        IEnumerator Move()
        {
            yield return new WaitForSeconds(2);
            isLeft = true;
            isRight = false;

            yield return new WaitForSeconds(2);
            isLeft = false;
            isRight = true;

            StartCoroutine(Move());
        }
    }

}
