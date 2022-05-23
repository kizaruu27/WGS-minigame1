using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    [SerializeField] GameObject[] itemsPrefabs;

    [SerializeField] float maxX, minX;

    [SerializeField] Transform spawnPoint;

    [SerializeField] bool isRandom;

    private void Start()
    {
        if (isRandom)
        {
            spawnPoint.position = new Vector3 (Random.Range(minX, maxX), spawnPoint.position.y, spawnPoint.position.z);
            Instantiate(itemsPrefabs[Random.Range(0, itemsPrefabs.Length)], transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(itemsPrefabs[Random.Range(0, itemsPrefabs.Length)], transform.position, Quaternion.identity);
        }
    }
}
