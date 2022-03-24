using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multiplayer_FinishCheck : MonoBehaviour
{
    public Multiplayer_ScriptableValue FinishValidation;
    [SerializeField] GameObject finishUI;
    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (view.IsMine)
        {
            if (col.gameObject.tag == "Player")
            {
                FinishValidation.isFinish = true;
            }
        }
    }
}
