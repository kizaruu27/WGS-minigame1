using System.Collections;
using UnityEngine;
using RunMinigames.Interface;
using Photon.Pun;
using RunMinigames.Manager.Game;

namespace RunMinigames.Mechanics.Items
{
    public class ObstaclesItem : InteractableItem
    {
        GameManager type;
        PhotonView view;

        private new void Awake()
        {
            isObstacles = true;

            GameObject gameManager = GameObject.Find("GameManager");
            type = gameManager.GetComponent<GameManager>();
        }

        public override IEnumerator OnCollideBehaviour(ICharacterItem character)
        {
            character.IsItemSpeedActive = false;
            character.MaxSpeed = SpeedCharacter;

            yield return new WaitForSeconds(LongTimeBehaviour);

            character.MaxSpeed = 10;
        }
    }
}
