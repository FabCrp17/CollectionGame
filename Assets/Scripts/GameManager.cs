using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public NetworkVariable<int> timeRemaining = new NetworkVariable<int>(300);
    public NetworkVariable<bool> isGameActive = new NetworkVariable<bool>(false);

    public NetworkVariable<bool> isWaitingForPlayers = new NetworkVariable<bool>(true);

    private float internalTimer;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            isWaitingForPlayers.Value = true;
            isGameActive.Value = true;
            internalTimer = 100f;

            NetworkManager.Singleton.OnClientDisconnectCallback += HandlePlayerDisconnect;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandlePlayerDisconnect;
        }
    }

    private void HandlePlayerDisconnect(ulong clientId)
    {
        isWaitingForPlayers.Value = true;
        isGameActive.Value = false;

        internalTimer = 300f;
        timeRemaining.Value = 300;
    }

    private void Update()
    {
        if (!IsServer) return;

        if (isWaitingForPlayers.Value)
        {
            if (NetworkManager.Singleton.ConnectedClients.Count >= 2)
            {
                isWaitingForPlayers.Value = false;
                isGameActive.Value = true;
            }
            return;
        }

        if (isGameActive.Value)
        {
            internalTimer -= Time.deltaTime;
            int secondsLeft = Mathf.CeilToInt(internalTimer);

            if (secondsLeft != timeRemaining.Value)
            {
                timeRemaining.Value = secondsLeft;
            }

            if (internalTimer <= 0)
            {
                timeRemaining.Value = 0;
                isGameActive.Value = false;
            }
        }
    }

}