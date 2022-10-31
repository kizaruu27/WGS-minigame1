using System.Collections;
using UnityEngine;
using RunMinigames.Interface;
using Photon.Pun;

namespace RunMinigames.Mechanics.Items
{
    public class M1_StopItem : M1_InteractableItem
    {
        public override IEnumerator OnCollideBehaviour(M1_ICharacterItem m1ICharacter)
        {
            M1_ObjectSoundManager.instance.PlayStopItemSound();
            
            Instantiate(itemEffect, transform.position, Quaternion.identity);

            itemMesh.SetActive(false);
            sphereCollider.enabled = false;

            m1ICharacter.CanMove = false;
            m1ICharacter.IsItemSpeedActive = false;
            m1ICharacter.CharSpeed = 0;

            yield return new WaitForSeconds(LongTimeBehaviour);

            m1ICharacter.CanMove = true;

            if (pv.IsMine)
                PhotonNetwork.Destroy(gameObject);
        }
    }
}
