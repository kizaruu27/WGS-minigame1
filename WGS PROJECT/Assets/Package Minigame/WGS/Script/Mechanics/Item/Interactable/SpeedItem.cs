using System.Collections;
using UnityEngine;
using RunMinigames.Interface;

namespace RunMinigames.Mechanics.Items
{
    public class SpeedItem : InteractableItem
    {
        float PrevPlayerSpeed;
        float PrevNPCSpeed;

        public override IEnumerator OnCollideBehaviour(ICharacterItem character)
        {
            mesh.enabled = false;
            sphereCollider.enabled = false;

            if (character.CanMove)
            {
                PrevPlayerSpeed = character.CharSpeed;
                character.CharSpeed += SpeedCharacter;
                character.IsItemSpeedActive = true;

                yield return new WaitForSeconds(LongTimeBehaviour);

                character.CharSpeed = PrevPlayerSpeed;
                character.IsItemSpeedActive = false;

                Destroy(gameObject);
            }

            character.IsItemSpeedActive = false;
            yield return new WaitForSeconds(0);
        }
    }
}
