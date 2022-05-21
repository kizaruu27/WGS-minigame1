using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeTrigger : MonoBehaviour
{
    [SerializeField] ShakeCamera shakeCamera;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Obstacle" || col.tag == "Stop")
        {
            shakeCamera.Shake();
        }
    }
}
