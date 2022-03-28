using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MultiplayerGameManager : MonoBehaviour
{
    public GameObject PauseUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUI.SetActive(true);
        }
    }

    public void QuitGame(string targetScene)
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(targetScene);
    }

    public void ResumeGame()
    {
        PauseUI.SetActive(false);
    }
}
