using System.Collections;
using UnityEngine;
using RunMinigames.Interface;
using Photon.Pun;

namespace RunMinigames.Mechanics.Items
{
    public class M1_SpeedItem : M1_InteractableItem
    {
        float PrevPlayerSpeed;
        float PrevNPCSpeed;

        public override IEnumerator OnCollideBehaviour(M1_ICharacterItem m1ICharacter)
        {
            M1_ObjectSoundManager.instance.PlaySpeedItemSound();
            
            itemMesh.SetActive(false);
            sphereCollider.enabled = false;

            Instantiate(itemEffect, transform.position, Quaternion.identity);

            if (m1ICharacter.CanMove)
            {
                PrevPlayerSpeed = m1ICharacter.CharSpeed;
                m1ICharacter.CharSpeed += SpeedCharacter;
                m1ICharacter.IsItemSpeedActive = true;

                yield return new WaitForSeconds(LongTimeBehaviour);

                m1ICharacter.CharSpeed = PrevPlayerSpeed;
                m1ICharacter.IsItemSpeedActive = false;

                if (pv.IsMine)
                    PhotonNetwork.Destroy(gameObject);
            }

            m1ICharacter.IsItemSpeedActive = false;
            yield return new WaitForSeconds(0);
        }
    }
}
