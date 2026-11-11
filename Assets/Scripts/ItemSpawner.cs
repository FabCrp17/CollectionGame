using Unity.Netcode;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{
    public static ItemSpawner Instance;

    public GameObject itemPrefab;
    public ItemData[] availableItems;
    public Vector2 spawnArea = new Vector2(8, 8);
    public float spawnInterval = 3f;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            InvokeRepeating(nameof(SpawnItem), 1f, spawnInterval);
        }
    }

    private void SpawnItem()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            1f,
            Random.Range(-spawnArea.y, spawnArea.y)
        );

        GameObject newItem = Instantiate(itemPrefab, randomPos, Quaternion.identity);
        int randomIndex = Random.Range(0, availableItems.Length);

        newItem.GetComponent<NetworkObject>().Spawn();

        CollectibleItem collectible = newItem.GetComponent<CollectibleItem>();
        collectible.itemTypeIndex.Value = randomIndex;
        collectible.data = availableItems[randomIndex];
    }
}