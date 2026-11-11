using Unity.Netcode;
using UnityEngine;

public class CollectibleItem : NetworkBehaviour
{
    public ItemData data;
    public NetworkVariable<int> itemTypeIndex = new NetworkVariable<int>(-1);

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        ApplyItemData(itemTypeIndex.Value);

        itemTypeIndex.OnValueChanged += (oldValue, newValue) =>
        {
            ApplyItemData(newValue);
        };
    }

    private void ApplyItemData(int index)
    {
        if (ItemSpawner.Instance != null && index >= 0 && index < ItemSpawner.Instance.availableItems.Length)
        {
            data = ItemSpawner.Instance.availableItems[index];
            meshRenderer.material.color = data.itemColor;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (other.CompareTag("Player"))
        {
            ulong clientId = other.GetComponent<NetworkObject>().OwnerClientId;
            ScoreManager.Instance.AddScore(clientId, data.scoreValue);
            GetComponent<NetworkObject>().Despawn();
        }
    }
}