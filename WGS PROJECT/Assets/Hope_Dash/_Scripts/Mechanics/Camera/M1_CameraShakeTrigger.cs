using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RunMinigames.Manager.Characters;
using RunMinigames.Mechanics.Characters;
using Photon.Pun;

namespace RunMinigames.Mechanics.Cam
{
    public class M1_CameraShakeTrigger : MonoBehaviour
    {
        [SerializeField] M1_ShakeCamera m1ShakeCamera;

        List<string> item = new List<string> { "Obstacle", "Stop" };
        PhotonView view;


        private void Awake()
        {
            m1ShakeCamera = GameObject.FindObjectOfType<M1_ShakeCamera>();
            view = GetComponent<PhotonView>();
        }


        private void OnTriggerEnter(Collider col)
        {


            if (item.Contains(col.tag) && view.IsMine)
            {
                m1ShakeCamera.Shake();
            }
        }
    }
}

