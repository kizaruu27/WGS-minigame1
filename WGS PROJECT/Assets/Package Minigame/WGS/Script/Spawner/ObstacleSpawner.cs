using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using RunMinigames.Models;
using RunMinigames.Manager.Game;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] float xMin;
    [SerializeField] float xMax;

    [SerializeField] GameObject obstaclePrefabs;
    [SerializeField] Transform obstaclesGroup;
    [SerializeField] Transform spawnPoint;



    // Start is called before the first frame update
    void Start()
    {
        spawnPoint.position = new Vector3(Random.Range(xMin, xMax), spawnPoint.position.y, spawnPoint.position.z);

        MPrefabs prefabs = new MPrefabs(obstaclePrefabs, obstaclePrefabs.name, spawnPoint.position, Quaternion.identity);

        var obsSpawner = (GameManager.instance.IsMultiplayer) ?
            PhotonNetwork.Instantiate(prefabs.name, prefabs.position, prefabs.quartenion) :
            Instantiate(prefabs.gameobject, prefabs.position, prefabs.quartenion);

        obsSpawner.transform.SetParent(obstaclesGroup);
    }
}
