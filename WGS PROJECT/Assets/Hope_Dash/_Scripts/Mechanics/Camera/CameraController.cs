using UnityEngine;

namespace RunMinigames.Mechanics.Cam
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;
        public Transform Player;

        void LateUpdate()
        {
            transform.position = Player.position;
        }
    }
}

