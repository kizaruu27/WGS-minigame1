using System.Collections;
using UnityEngine;

public class MoveItem : MonoBehaviour
{
    [SerializeField] public float speed = 1.5f;
    Rigidbody rb;
    [SerializeField] bool isRight, isLeft;

    private void Awake() => rb = gameObject?.GetComponent<Rigidbody>();

    private void Start()
    {
        isRight = true;
        isLeft = false;
        StartCoroutine(Move());
    }

    private void FixedUpdate()
    {
        if (isRight && !isLeft)
        {
            rb.velocity = transform.right * speed;
            isLeft = false;
        }

        if (isLeft && !isRight)
        {
            rb.velocity = transform.forward * speed;
            isRight = false;
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
