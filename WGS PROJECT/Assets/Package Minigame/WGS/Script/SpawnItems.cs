using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    [SerializeField] GameObject[] itemsPrefabs;
    [SerializeField] float spawnTimes;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnItem());
    }

    IEnumerator SpawnItem()
    {
        GameObject Spawing = Instantiate(itemsPrefabs[Random.Range(0, itemsPrefabs.Length)], transform.position, transform.rotation);
        Spawing.transform.SetParent(transform);
        yield return new WaitForSeconds(spawnTimes);

        StartCoroutine(SpawnItem());
    }
}
