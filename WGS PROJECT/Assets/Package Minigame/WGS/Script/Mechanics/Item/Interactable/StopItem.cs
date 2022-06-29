using System.Collections;
using UnityEngine;
using RunMinigames.Interface;
using Photon.Pun;

namespace RunMinigames.Mechanics.Items
{
    public class StopItem : InteractableItem
    {
        public override IEnumerator OnCollideBehaviour(ICharacterItem character)
        {
            Instantiate(itemEffect, transform.position, Quaternion.identity);

            mesh.enabled = false;
            sphereCollider.enabled = false;

            character.CanMove = false;
            character.IsItemSpeedActive = false;
            character.CharSpeed = 0;

            yield return new WaitForSeconds(LongTimeBehaviour);

            character.CanMove = true;

            if (pv.IsMine)
                PhotonNetwork.Destroy(gameObject);
        }
    }
}
