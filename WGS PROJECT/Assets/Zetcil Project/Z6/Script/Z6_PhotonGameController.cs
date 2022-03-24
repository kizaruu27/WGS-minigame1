using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Z6_PhotonGameController : MonoBehaviour
{

    public string PrefabsFolder = "PhotonPrefabs";
    public string PrefabsObject = "PhotonPlayerDefault";
    public GameObject SpawnPoint; 

    // Start is called before the first frame update
    void Start()
    {
        //Photon #10
        CreatePlayer();   
    }

    void CreatePlayer() 
    {
        Debug.Log("Game Controller: Create Player");
        if (PlayerPrefs.HasKey("CurrentPlayer"))
        {
            PrefabsObject = PlayerPrefs.GetString("CurrentPlayer");
            SpawnPoint = GameObject.Find("Spawn_"+ PrefabsObject);
        }
        PhotonNetwork.Instantiate(Path.Combine(PrefabsFolder, PrefabsObject), SpawnPoint.transform.position, SpawnPoint.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
