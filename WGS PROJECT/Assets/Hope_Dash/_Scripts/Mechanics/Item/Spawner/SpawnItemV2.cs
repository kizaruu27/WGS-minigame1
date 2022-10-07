using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemV2 : MonoBehaviour
{
    [SerializeField] private GameObject[] itemPrefabs;
    
    void Start()
    {
        Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Length)], transform.position, Quaternion.identity);
    }
    
}
