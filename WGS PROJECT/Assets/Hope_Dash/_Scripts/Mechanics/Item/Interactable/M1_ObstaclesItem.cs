using System.Collections;
using UnityEngine;
using RunMinigames.Interface;
using Photon.Pun;
using RunMinigames.Manager.Game;

namespace RunMinigames.Mechanics.Items
{
    public class M1_ObstaclesItem : M1_InteractableItem
    {
        M1_GameManager type;
        PhotonView view;

        private new void Awake()
        {
            isObstacles = true;

            GameObject gameManager = GameObject.Find("GameManager");
            type = gameManager.GetComponent<M1_GameManager>();
        }

        public override IEnumerator OnCollideBehaviour(M1_ICharacterItem m1ICharacter)
        {
            M1_ObjectSoundManager.instance.PlayObstacleSound();
            m1ICharacter.IsItemSpeedActive = false;
            m1ICharacter.MaxSpeed = SpeedCharacter;

            yield return new WaitForSeconds(LongTimeBehaviour);

            m1ICharacter.MaxSpeed = 10;
        }
    }
}
