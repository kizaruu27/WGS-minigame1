using UnityEngine;
using RunMinigames.Interface;

namespace RunMinigames.Mechanics.Checkpoint
{
    public class GameCheckpoint : MonoBehaviour
    {
        public bool isFinishLine = false;
        public bool stopAfterFinish = false;
        public int checkPointNumber = 1;

        private void Awake()
        {
            var name = gameObject.name.Split(' ');

            for (var i = 0; i < name.Length; i++)
                if (i == 1) checkPointNumber = int.Parse(name[i]);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.TryGetComponent(out ICharacterItem character) && stopAfterFinish)
            {
                character.CanMove = false;
                character.MaxSpeed = 0;
            }
        }
    }
}

