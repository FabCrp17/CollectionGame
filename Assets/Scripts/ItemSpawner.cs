using Unity.Netcode;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{
    public static ItemSpawner Instance;

    public GameObject itemPrefab;
    public ItemData[] availableItems;

    [Header("spawn settings")]
    public Vector2 spawnArea = new Vector2(8, 8);
    public float spawnInterval = 6f;
    public float checkRadius = 1.0f;

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

        if (GameManager.Instance == null) return;

        if (GameManager.Instance.isWaitingForPlayers.Value) return;

        if (!GameManager.Instance.isGameActive.Value)
        {
            CancelInvoke(nameof(SpawnItem));
            return;
        }

        Vector3 spawnPosition = Vector3.zero;
        bool validPosition = false;
        int attempts = 0;

        while (!validPosition && attempts < 10)
        {
            spawnPosition = transform.position + new Vector3(
                Random.Range(-spawnArea.x, spawnArea.x),
                1f,
                Random.Range(-spawnArea.y, spawnArea.y)
            );

            Collider[] colliders = Physics.OverlapSphere(spawnPosition, checkRadius);
            bool isOccupied = false;

            foreach (Collider col in colliders)
            {
                
                if (col.CompareTag("Player") || col.GetComponent<CollectibleItem>() != null)
                {
                    isOccupied = true;
                    break;
                }
            }

            if (!isOccupied)
            {
                validPosition = true; 
            }

            attempts++;
        }

        if (!validPosition) return;

        GameObject newItem = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        int randomIndex = Random.Range(0, availableItems.Length);

        newItem.GetComponent<NetworkObject>().Spawn();

        CollectibleItem collectible = newItem.GetComponent<CollectibleItem>();
        collectible.itemTypeIndex.Value = randomIndex;
        collectible.data = availableItems[randomIndex];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position, new Vector3(spawnArea.x * 2, 2, spawnArea.y * 2));
    }
}


