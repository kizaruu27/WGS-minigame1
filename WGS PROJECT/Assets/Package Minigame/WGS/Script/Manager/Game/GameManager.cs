using UnityEngine;
using RunMinigames.Interface;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using Photon.Pun;

namespace RunMinigames.Manager.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Game Type")]
        public bool IsMultiplayer;

        [Header("Instantiation Prefabs")]
        public List<GameObject> Prefabs;

        private void Awake()
        {
            instance = this;
            // PreparePrefabsPool();

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

        public void PreparePrefabsPool()
        {
            DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
            if (pool != null && this.Prefabs != null)
            {
                foreach (GameObject prefab in this.Prefabs)
                {
                    pool.ResourceCache.Add(prefab.name, prefab);
                }
            }
        }
    }
}