using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RunMinigames.Manager.Characters;
using Photon.Pun;

namespace RunMinigames.Mechanics.Cam
{
    public class CameraShakeTrigger : MonoBehaviour
    {
        [SerializeField] ShakeCamera shakeCamera;

        List<string> item = new List<string> { "Obstacle", "Stop" };
        PhotonView view;


        private void Awake()
        {
            shakeCamera = GameObject.FindObjectOfType<ShakeCamera>();
            view = GetComponent<PhotonView>();
        }


        private void OnTriggerEnter(Collider col)
        {

            if (item.Contains(col.tag) && view.IsMine)
            {
                shakeCamera.Shake();
            }
        }
    }
}

