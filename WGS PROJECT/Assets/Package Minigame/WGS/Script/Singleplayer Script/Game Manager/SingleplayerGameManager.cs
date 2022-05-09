using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleplayerGameManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject pauseButton;

    private void Awake()
    {
        pauseButton.gameObject.SetActive(CheckPlatform.isAndroid || CheckPlatform.isIos);
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;

    }


}
