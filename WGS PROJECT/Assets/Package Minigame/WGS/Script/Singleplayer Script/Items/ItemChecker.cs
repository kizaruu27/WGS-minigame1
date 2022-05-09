using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            Singleplayer_ItemSpeed.instance.speedUp = false;
        }
    }
}
