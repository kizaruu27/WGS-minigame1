using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Zetcode_LoadSceneClick : MonoBehaviour
{
    // Start is called before the first frame update

    public void LoadScene(string aScene)
    {
        SceneManager.LoadScene(aScene);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

