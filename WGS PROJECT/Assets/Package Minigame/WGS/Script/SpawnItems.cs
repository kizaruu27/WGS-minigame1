using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    [SerializeField] GameObject itemsPrefabs;
    [SerializeField] float spawnTimes;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnItem());
    }

    IEnumerator SpawnItem() {
        Instantiate(itemsPrefabs, transform.position, transform.rotation);
        yield return new WaitForSeconds(spawnTimes);

        StartCoroutine(SpawnItem());
    }
}
