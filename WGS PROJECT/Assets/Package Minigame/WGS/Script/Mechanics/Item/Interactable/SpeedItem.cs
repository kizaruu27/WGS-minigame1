using System.Collections;
using UnityEngine;
using RunMinigames.Interface;
using Photon.Pun;

namespace RunMinigames.Mechanics.Items
{
    public class SpeedItem : InteractableItem
    {
        float PrevPlayerSpeed;
        float PrevNPCSpeed;

        public override IEnumerator OnCollideBehaviour(ICharacterItem character)
        {
            itemMesh.SetActive(false);
            sphereCollider.enabled = false;

            Instantiate(itemEffect, transform.position, Quaternion.identity);

            if (character.CanMove)
            {
                PrevPlayerSpeed = character.CharSpeed;
                character.CharSpeed += SpeedCharacter;
                character.IsItemSpeedActive = true;

                yield return new WaitForSeconds(LongTimeBehaviour);

                character.CharSpeed = PrevPlayerSpeed;
                character.IsItemSpeedActive = false;

                if (pv.IsMine)
                    PhotonNetwork.Destroy(gameObject);
            }

            character.IsItemSpeedActive = false;
            yield return new WaitForSeconds(0);
        }
    }
}
