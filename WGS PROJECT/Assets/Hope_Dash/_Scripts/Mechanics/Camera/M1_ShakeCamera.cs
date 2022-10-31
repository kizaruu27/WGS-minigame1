using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace RunMinigames.Mechanics.Cam
{
    public class M1_ShakeCamera : MonoBehaviour
    {
        Vector3 cameraInitialPos;
        public float shakeMagnitude = 0.05f, shakeTime = 0.5f;
        public Camera mainCamera;

        [SerializeField] Transform cameraRig;
        [SerializeField] Transform player;

        private void Awake()
        {
            player = GameObject.FindObjectOfType<M1_CameraController>().Player;
            cameraInitialPos = mainCamera.transform.localPosition;
        }

        public void Shake()
        {
            InvokeRepeating("StartCameraShaking", 0, 0.005f);
            Invoke("StopCameraShaking", shakeTime);
            cameraRig.position = player.position;
        }

        void StartCameraShaking()
        {
            float cameraShakingOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
            float cameraShakingOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
            Vector3 cameraIntermadiatePos = mainCamera.transform.localPosition;
            cameraIntermadiatePos.x += cameraShakingOffsetX;
            cameraIntermadiatePos.y += cameraShakingOffsetY;
            mainCamera.transform.localPosition = cameraIntermadiatePos;

        }

        void StopCameraShaking()
        {
            CancelInvoke("StartCameraShaking");
            mainCamera.transform.localPosition = cameraInitialPos;

        }
    }
}

