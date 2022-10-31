using UnityEngine;

namespace RunMinigames.Mechanics.Cam
{
    public class M1_CameraController : MonoBehaviour
    {
        public static M1_CameraController instance;
        public Transform Player;

        void LateUpdate()
        {
            transform.position = Player.position;
        }
    }
}

