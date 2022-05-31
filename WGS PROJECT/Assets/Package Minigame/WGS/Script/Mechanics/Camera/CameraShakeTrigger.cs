using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunMinigames.Mechanics.Cam
{
    public class CameraShakeTrigger : MonoBehaviour
    {
        [SerializeField] ShakeCamera shakeCamera;

        private void Awake() => shakeCamera = GameObject.FindObjectOfType<ShakeCamera>();


        private void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Obstacle" || col.tag == "Stop")
            {
                shakeCamera.Shake();
            }
        }
    }
}

