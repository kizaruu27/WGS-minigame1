using UnityEngine;
using RunMinigames.Interface;
using System.Linq;

namespace RunMinigames.Manager.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public bool IsMultiplayer;

        private void Awake() => instance = this;

        public void SetActiveCharacter()
        {
            var characters = FindObjectsOfType<MonoBehaviour>().OfType<ICharacterItem>();

            foreach (var character in characters) character.Active = true;

        }
    }
}