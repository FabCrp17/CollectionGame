using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public NetworkVariable<int> scorePlayer1 = new NetworkVariable<int>(0);
    public NetworkVariable<int> scorePlayer2 = new NetworkVariable<int>(0);

    public static ScoreManager Instance;

    private void Awake() => Instance = this;

    public void AddScore(ulong clientId, int amount)
    {
        if (!IsServer) return;

        if (clientId == 0) scorePlayer1.Value += amount;
        else scorePlayer2.Value += amount;
    }
}