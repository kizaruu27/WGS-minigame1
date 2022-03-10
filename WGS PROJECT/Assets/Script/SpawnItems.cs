using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    [SerializeField] GameObject itemsPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(itemsPrefabs, transform.position, transform.rotation);
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
