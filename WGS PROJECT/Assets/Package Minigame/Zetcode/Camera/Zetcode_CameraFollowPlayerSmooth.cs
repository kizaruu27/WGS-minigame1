using UnityEngine;

public class Zetcode_CameraFollowPlayerSmooth : MonoBehaviour
{
    public GameObject TargetPlayer;
    public Vector3 Offset;
    public float SmoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        Offset = transform.position - TargetPlayer.transform.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = TargetPlayer.transform.position + Offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        transform.LookAt(TargetPlayer.transform);
    }
}