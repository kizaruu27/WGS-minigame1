
using UnityEngine;
using Photon.Pun;
using RunMinigames.Manager.Game;
using RunMinigames.Models;



public class M1_SpawnItems : MonoBehaviour
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
      var go = itemsPrefabs[Random.Range(0, itemsPrefabs.Length)];

      if (isRandom)
      {
        spawnPoint.position = new Vector3(Random.Range(minX, maxX), spawnPoint.position.y, spawnPoint.position.z);

        M1_MPrefabs prefabs = new M1_MPrefabs(go, go.name, spawnPoint.position, Quaternion.identity);

        var insItems = (M1_GameManager.instance.IsMultiplayer) ?
            PhotonNetwork.InstantiateRoomObject(prefabs.name, prefabs.position, prefabs.quartenion) :
            Instantiate(prefabs.gameobject, prefabs.position, prefabs.quartenion);

        insItems.transform.SetParent(itemsGroup);
      }
      else
      {
        M1_MPrefabs prefabs = new M1_MPrefabs(go, go.name, transform.position, Quaternion.identity);

        var insItems = (M1_GameManager.instance.IsMultiplayer) ?
            PhotonNetwork.InstantiateRoomObject(prefabs.name, prefabs.position, prefabs.quartenion) :
            Instantiate(prefabs.gameobject, prefabs.position, prefabs.quartenion);

        insItems.transform.SetParent(itemsGroup);
      }
    }
  }
}
