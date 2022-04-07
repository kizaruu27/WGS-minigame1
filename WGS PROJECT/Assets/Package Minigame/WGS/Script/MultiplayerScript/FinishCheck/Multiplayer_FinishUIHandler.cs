using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Multiplayer_FinishUIHandler : MonoBehaviour
{
    public Multiplayer_ScriptableValue finishValidation;
    public GameObject finishUI;
    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            if (finishValidation.isFinish)
            {
                finishUI.SetActive(true);
            }
        }
    }
}
