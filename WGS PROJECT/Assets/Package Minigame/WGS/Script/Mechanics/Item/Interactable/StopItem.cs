using System.Collections;
using UnityEngine;
using RunMinigames.Interface;

namespace RunMinigames.Mechanics.Interactable
{
    public class StopItem : InteractableItem
    {
        public override IEnumerator OnCollideBehaviour(ICharacterItem character)
        {
            mesh.enabled = false;
            sphereCollider.enabled = false;

            character.CanMove = false;
            character.IsItemSpeedActive = false;
            character.CharSpeed = 0;

            yield return new WaitForSeconds(LongTimeBehaviour);

            character.CanMove = true;
            Destroy(gameObject);
        }
    }
}
