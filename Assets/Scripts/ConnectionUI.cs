using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour
{
    public TMP_InputField ipInputField;
    public Button hostButton;
    public Button clientButton;

    private void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        gameObject.SetActive(false);
    }

    private void StartClient()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ipInputField.text;
        NetworkManager.Singleton.StartClient();
        gameObject.SetActive(false);
    }
}