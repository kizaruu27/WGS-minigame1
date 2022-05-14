using UnityEngine;


namespace RunMinigames.Mechanics.Cam
{
    public class CameraController : MonoBehaviour
    {
        public Transform Player;
        public Vector3 offset;

        public float smoothFactor = 0.5f;

        void Start()
        {
            offset = transform.position - Player.position;
        }

        void LateUpdate()
        {
            Vector3 newPosition = Player.position + offset;
            transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);
        }
    }
}

