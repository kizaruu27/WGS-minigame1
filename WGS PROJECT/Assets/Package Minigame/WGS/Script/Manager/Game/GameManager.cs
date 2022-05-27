using UnityEngine;
using RunMinigames.Interface;
using System.Linq;
using System.Collections;
using System;
using Photon.Pun;

namespace RunMinigames.Manager.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Game Type")]
        public bool IsMultiplayer;

        private void Awake()
        {
            instance = this;
        }

        public void SetActiveCharacter()
        {
            var characters = FindObjectsOfType<MonoBehaviour>().OfType<ICharacterItem>();

            foreach (var character in characters) character.Active = true;
        }

        public IEnumerator WaitAllPlayerReady(Action ActionMethod)
        {
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Length == (int)PhotonNetwork.PlayerList.Length);

            ActionMethod();
        }
    }
}