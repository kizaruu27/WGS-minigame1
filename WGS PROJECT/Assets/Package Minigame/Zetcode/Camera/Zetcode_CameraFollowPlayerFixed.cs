using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Zetcode_CameraFollowPlayerFixed : MonoBehaviour
{
    public static Zetcode_CameraFollowPlayerFixed cameraFollow;
    public GameObject TargetPlayer;
    private Vector3 offset;
    private Vector3 newtrans;
    public bool isShake;

    PhotonView view;


    private void Awake()
    {
        if (cameraFollow != null)
        {
            Destroy(gameObject);
        }
        else
        {
            cameraFollow = this;
        }
    }

    void Start()
    {
        offset.x = transform.position.x - TargetPlayer.transform.position.x;
        offset.z = transform.position.z - TargetPlayer.transform.position.z;
        newtrans = transform.position;
    }

    void LateUpdate()
    {
        newtrans.x = TargetPlayer.transform.position.x + offset.x;
        newtrans.z = TargetPlayer.transform.position.z + offset.z;
        transform.position = newtrans;

        if (isShake && view.IsMine)
        {
            StartCoroutine(ShakeCamera());
        }

    }

    IEnumerator ShakeCamera()
    {
        transform.position = new Vector3(transform.position.x + Time.deltaTime * 50, transform.position.y, transform.position.z);

        yield return new WaitForSeconds(0.2f);
        isShake = false;
    }
}