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
            ObjectSoundManager.instance.PlayStopItemSound();
            Instantiate(itemEffect, transform.position, Quaternion.identity);

            itemMesh.SetActive(false);
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
