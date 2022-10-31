using UnityEngine;

public class M1_GamePause : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject pauseButton;

    private void Awake()
    {
        pauseButton.gameObject.SetActive(M1_CheckPlatform.isAndroid || M1_CheckPlatform.isIos);
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