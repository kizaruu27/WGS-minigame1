
using UnityEngine;
using Photon.Pun;

namespace RunMinigames.Mechanics.Cam
{
    public class CameraShake : MonoBehaviour
    {
        public float power = 0.7f;
        public float duration = 1f;
        public Transform cam;
        public float slowDownAmmount = 1f;
        public bool Shake = false;

        Vector3 starPos;
        float initialDuration;

        PhotonView view;


        private void Awake() => view = GetComponent<PhotonView>();

        private void Start()
        {
            cam = Camera.main.transform;
            starPos = cam.localPosition;
            initialDuration = duration;
        }

        private void Update()
        {
            if (view.IsMine)
            {
                if (duration > 0)
                {
                    cam.localPosition = starPos + Random.insideUnitSphere * power;
                    duration -= Time.deltaTime * slowDownAmmount;
                }
                else
                {
                    //Shake = false;
                    duration = initialDuration;
                    cam.localPosition = starPos;
                }
            }
        }
    }

}
