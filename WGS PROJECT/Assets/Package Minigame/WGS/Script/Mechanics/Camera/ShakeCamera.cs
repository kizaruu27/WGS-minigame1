using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunMinigames.Mechanics.Cam
{
    public class ShakeCamera : MonoBehaviour
    {
        Vector3 cameraInitialPos;
        public float shakeMagnitude = 0.05f, shakeTime = 0.5f;
        public Camera mainCamera;

        [SerializeField] Transform cameraRig;
        [SerializeField] Transform player;

        private void Awake()
        {
            player = GameObject.FindObjectOfType<CameraController>().Player;
        }

        void Update()
        {

            if (Input.GetMouseButtonDown(1))
            {
                Shake();
            }
            cameraInitialPos = mainCamera.transform.position;

        }

        public void Shake()
        {
            cameraInitialPos = mainCamera.transform.position;
            InvokeRepeating("StartCameraShaking", 0, 0.005f);
            Invoke("StopCameraShaking", shakeTime);
            cameraRig.position = player.position;
        }

        void StartCameraShaking()
        {
            float cameraShakingOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
            float cameraShakingOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
            Vector3 cameraIntermadiatePos = mainCamera.transform.position;
            cameraIntermadiatePos.x += cameraShakingOffsetX;
            cameraIntermadiatePos.y += cameraShakingOffsetY;
            mainCamera.transform.position = cameraIntermadiatePos;

        }

        void StopCameraShaking()
        {
            CancelInvoke("StartCameraShaking");
            mainCamera.transform.position = cameraInitialPos;

        }


    }
}

