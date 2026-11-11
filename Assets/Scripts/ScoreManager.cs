using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public NetworkVariable<int> scorePlayer1 = new NetworkVariable<int>(0);
    public NetworkVariable<int> scorePlayer2 = new NetworkVariable<int>(0);

    public static ScoreManager Instance;

    private void Awake() => Instance = this;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            scorePlayer1.Value = 0;
            scorePlayer2.Value = 0;
            
            NetworkManager.Singleton.OnClientDisconnectCallback += ResetScoreOnDisconnect;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= ResetScoreOnDisconnect;
        }
    }

    private void ResetScoreOnDisconnect(ulong clientId)
    {

        scorePlayer1.Value = 0;
        scorePlayer2.Value = 0;
    }

    public void AddScore(ulong clientId, int amount)
    {
        if (!IsServer) return;

        if (clientId == 0) scorePlayer1.Value += amount;
        else scorePlayer2.Value += amount;
    }
}