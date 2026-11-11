using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public NetworkVariable<int> timeRemaining = new NetworkVariable<int>(300);
    public NetworkVariable<bool> isGameActive = new NetworkVariable<bool>(false);

    private float internalTimer;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            isGameActive.Value = true;
            internalTimer = 300f; 
        }
    }

    private void Update()
    {
        if (!IsServer || !isGameActive.Value) return;

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