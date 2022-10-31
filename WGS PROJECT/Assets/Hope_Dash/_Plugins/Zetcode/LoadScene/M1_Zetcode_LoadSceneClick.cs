using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M1_Zetcode_LoadSceneClick : MonoBehaviour
{
    // Start is called before the first frame update

    public void LoadScene(string aScene)
    {
        SceneManager.LoadScene(aScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

