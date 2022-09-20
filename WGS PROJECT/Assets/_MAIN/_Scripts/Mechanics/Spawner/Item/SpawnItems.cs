
using UnityEngine;
using Photon.Pun;
using RunMinigames.Manager.Game;
using RunMinigames.Models;



public class SpawnItems : MonoBehaviour
{
    [SerializeField] GameObject[] itemsPrefabs;
    [SerializeField] float maxX, minX;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform itemsGroup;
    [SerializeField] bool isRandom;

    private void Start()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("is master");
            var go = itemsPrefabs[Random.Range(0, itemsPrefabs.Length)];

            if (isRandom)
            {
                spawnPoint.position = new Vector3(Random.Range(minX, maxX), spawnPoint.position.y, spawnPoint.position.z);

                MPrefabs prefabs = new MPrefabs(go, go.name, spawnPoint.position, Quaternion.identity);

                var insItems = (GameManager.instance.IsMultiplayer) ?
                    PhotonNetwork.InstantiateRoomObject(prefabs.name, prefabs.position, prefabs.quartenion) :
                    Instantiate(prefabs.gameobject, prefabs.position, prefabs.quartenion);

                insItems.transform.SetParent(itemsGroup);
            }
            else
            {
                MPrefabs prefabs = new MPrefabs(go, go.name, transform.position, Quaternion.identity);

                var insItems = (GameManager.instance.IsMultiplayer) ?
                    PhotonNetwork.InstantiateRoomObject(prefabs.name, prefabs.position, prefabs.quartenion) :
                    Instantiate(prefabs.gameobject, prefabs.position, prefabs.quartenion);

                insItems.transform.SetParent(itemsGroup);
            }
        }
    }
}
