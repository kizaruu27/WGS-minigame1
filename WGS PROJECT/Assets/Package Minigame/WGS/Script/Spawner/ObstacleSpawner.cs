using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] float xMin;
    [SerializeField] float xMax;

    [SerializeField] GameObject obstaclePrefabs;

    [SerializeField] Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint.position = new Vector3 (Random.Range(xMin, xMax), spawnPoint.position.y, spawnPoint.position.z);
        Instantiate(obstaclePrefabs, spawnPoint.position, Quaternion.identity);
    }
}